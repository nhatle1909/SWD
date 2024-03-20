using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using Repositories.Repository;
using Services.Interface;
using Services.Tool;
using Services.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Twilio.Http;
using ZstdSharp.Unsafe;
using static Repositories.ModelView.CartView;
using Request = Repositories.Model.Request;
namespace Services.Service
{
    public class TransactionService : ITransactionService
    {
        string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        string returnUrl = "http://localhost:3000/CheckPayment.html";
        string tmnCode = "8G7V30JH";
        string hashSecret = "KBAOJWHYLHYDNOLUXKUAAKMBILURCGPB";

        private readonly IHttpContextAccessor ipget;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        VNPayUtil pay = new VNPayUtil();
        public TransactionService(IUnitOfWork unit, IMapper mapper, IHttpContextAccessor _ipget, IEmailSender emailSender)
        {
            _unit = unit;
            _mapper = mapper;
            ipget = _ipget;
            _emailSender = emailSender;
        }
        public async Task<string> Payment(string TransactionId, int Price)
        {
            string random = SomeTool.GenerateId();
            string Tref = TransactionId + random;

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (Price * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", Tref); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return paymentUrl;
        }
        public async Task<(bool, string)> CheckPayment(string url)
        {

            var query = Utils.GetQueryString(url);

            string vnp_SecureHash = Utils.ExtractUrlParam(url, "vnp_SecureHash");
            string _id = Utils.ExtractUrlParam(url, "vnp_TxnRef").Substring(0, Utils.ExtractUrlParam(url, "vnp_TxnRef").Length - 5);
            string vnp_ResponseCode = Utils.ExtractUrlParam(url, "vnp_ResponseCode");
            string vnp_TransactionStatus = Utils.ExtractUrlParam(url, "vnp_TransactionStatus");


            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["TransactionStatus"], a => a.TransactionId.Equals(_id));


            bool checkSignature = pay.ValidateSignature(query, vnp_SecureHash, hashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong

                    if (item.FirstOrDefault().TransactionStatus.Equals("Pending"))
                    {
                        await UpdateStatusTransaction(_id, "Processing");
                        await UpdateInteriorQuantity(_id);
                        await VNPayPaymentRemain(_id);
                    }
                    if (item.FirstOrDefault().TransactionStatus.Equals("Processing"))
                    {
                        await UpdateStatusTransaction(_id, "Success");
                        await SendThanksMail(_id);
                    }
                    return (true, "Checkout Successfull");
                }
                else
                {
                    await UpdateStatusTransaction(_id, "Canceled");
                    return (false, "Failed");

                }
            }
            else
            {
                return (false, "Invalid signature");
            }
        }
        public async Task<(bool, object)> GetAllTransaction(string id)
        {

            string email = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"], a => a.AccountId.Equals(id))).FirstOrDefault().Email;
            if (email == null) return (false, null);
            IEnumerable<Transaction> trans = await _unit.TransactionRepo.GetByFilterAsync(a => a.Email.Equals(email));

            IEnumerable<Contract> contract = await _unit.ContractRepo.GetByFilterAsync(a => a.EmailOfCustomer.Equals(email));
            var transList = trans.OrderByDescending(t => t.CreatedAt).ToArray();
            var contractList = contract.OrderByDescending(c => c.CreatedAt).ToArray();

            //Data cần lấy : Transaction ID - TransactionStatus - Total Price - Contract File - Contract Create Date - Transaction Date

            var responses = new List<object>();
            for (int i = 0; i < trans.Count(); i++)
            {
                pay.ClearRequestData();
                var url = "";
                if (transList[i].TransactionStatus.Equals("Pending"))
                {
                    url = Payment(transList[i].TransactionId, transList[i].TotalPrice * 3 / 10).Result;
                }
                if (transList[i].TransactionStatus.Equals("Processing"))
                {
                    url = Payment(transList[i].TransactionId, transList[i].RemainPrice).Result;
                }
                responses.Add(new
                {
                    TransactionID = transList[i].TransactionId,
                    TransactionStatus = transList[i].TransactionStatus,
                    TotalPrice = transList[i].TotalPrice,
                    ContractDate = contractList[i].CreatedAt,
                    TransactionDate = transList[i].UpdatedAt,
                    URL = url,
                    ContractFile = contractList[i].ContractFile


                });

            }
            return (true, responses);
        }

        public async Task<string> AddPendingTransaction(string _id, AddCartView[] cartViews)
        {
            int TotalPrice = await CalculateTotalPrice(cartViews);
            IEnumerable<Request> contact = await _unit.ContactRepo.GetFieldsByFilterAsync(["Email"], a => a.RequestId.Equals(_id));
            Transaction Transaction = new Transaction
            {
                TransactionId = ObjectId.GenerateNewId().ToString(),
                RequestId = _id,
                TransactionStatus = "Pending",
                Email = contact.First().Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpiredDate = DateTime.Now.AddMonths(1),
                TransactionDetail = _mapper.Map<TransactionDetail[]>(cartViews),
                TotalPrice = TotalPrice,
                RemainPrice = TotalPrice * 7 / 10
            };
            await _unit.TransactionRepo.AddOneItem(Transaction);

            return Transaction.TransactionId;
        }


        public async Task<int> CalculateDeposit(string _id)
        {
            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["TotalPrice"], a => a.TransactionId.Equals(_id));
            int deposit = item.FirstOrDefault().TotalPrice * 3 / 10;
            return deposit;
        }
        public async Task<string> DeleteTransaction(string _id)
        {
            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["_id", "TransactionStatus"], a => a.TransactionId.Equals(_id) && a.TransactionStatus.Equals("Pending"));
            if (item.Any())
            {
                await _unit.TransactionRepo.RemoveItemByValue("TransactionId", _id);
                return "Delete Successful";
            }
            return "Item does not exist";
        }
        public async Task<string> DeleteExpiredTransaction(string[] _ids)
        {
            foreach (string _id in _ids)
            {
                IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["_id", "ExpiredDate"], a => a.TransactionId.Equals(_id));
                if (!item.Any())
                {
                    return "Transaction does not exist";
                }
            }
            foreach (string _id in _ids)
            {
                IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["_id", "ExpiredDate"], a => a.TransactionId.Equals(_id));
                if (item.Any() && DateTime.Now > item.FirstOrDefault().ExpiredDate)
                {
                    await _unit.TransactionRepo.RemoveItemByValue("TransactionId", _id);
                }
            }
            return "Delete Succesful";
        }
        Task<string> ITransactionService.DeleteExpiredTransaction(string[] _ids)
        {
            throw new NotImplementedException();
        }
        //----------------------------------------------------End Interface---------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------------//
        public async Task VNPayPaymentRemain(string Transactionid)
        {
            IEnumerable<Transaction> trans = await _unit.TransactionRepo.GetFieldsByFilterAsync(["Email"], a => a.TransactionId.Equals(Transactionid));

            string email = trans.FirstOrDefault().Email;
            int RemainPrice = await GetRemainPrice(Transactionid);
            string paymentUrl = await Payment(Transactionid, RemainPrice);

            string html = @"<!DOCTYPE html>
<html>
<head>
<title>Your Order Awaits!</title>
<style>
  body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
  }
  .container {
    width: 600px;
    margin: 50px auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 5px;
  }
  h1 {
    font-size: 24px;
    margin-bottom: 15px;
  }
  p {
    font-size: 16px;
    line-height: 1.5;
    margin-bottom: 10px;
  }
  a {
    color: #007bff;
    text-decoration: none;
    font-weight: bold;
  }
  .important {
    color: #b32828;
    font-weight: bold;
  }
</style>
</head>
<body>" +
"<div class=\"container\">\r\n    <h1>Thank you for your recent purchase!</h1>\r\n    <p>To complete your order, please proceed to the secure payment portal by clicking the button below:</p>\r\n    <p><a href='" + paymentUrl + "'>Make Payment Now</a></p>\r\n    <p class=\"important\">**Important:** This link will expire in 15 minutes after clicking. If you do not complete the payment within this timeframe, your order may be cancelled. Additionally, your order will be automatically cancelled if not checked out within one month.</p>\r\n    <p>If you have any questions or encounter any issues, please don't hesitate to contact us at <a href=\"\"mailto:support@yourcompany.com\"\">support@yourcompany.com</a>.</p>\r\n    <p>Thank you for your business!</p>\r\n  </div>\r\n</body>\r\n</html>\"";

            string subject = "Interior quotation system";

            await _emailSender.SendEmailAsync(email, subject, html);
            return;

        }
        public async Task SendThanksMail(string Transactionid)
        {
            var trans = (await _unit.TransactionRepo.GetFieldsByFilterAsync([], a => a.TransactionId.Equals(Transactionid))).First();
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(trans.RequestId))).First();

            getContact.StatusResponseOfStaff = Request.State.Completed;
            getContact.UpdatedAt = DateTime.Now;
            await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);


            string subject = "Interior quotation system";
            string body = $"Thank You for Using Our Service!" +
                $"<h3><strong>If you have any questions or encounter any issues, please don't hesitate to contact us at support@yourcompany.com.</strong></h3>" +
                $"Thank you for your business!";
            await _emailSender.SendEmailAsync(getContact.Email, subject, body);
            return;
        }
        public async Task UpdateInteriorQuantity(string _id)
        {
            IEnumerable<Transaction> items = await _unit.TransactionRepo.GetFieldsByFilterAsync(["TransactionDetail"], a => a.TransactionId.Equals(_id));

            foreach (TransactionDetail item in items.FirstOrDefault().TransactionDetail)
            {
                var interior = await _unit.InteriorRepo.GetByFilterAsync(i => i.InteriorId.Equals(item.InteriorId));
                Interior newQuantity = interior.First();
                newQuantity.Quantity = newQuantity.Quantity - item.Quantity;
                await _unit.InteriorRepo.UpdateItemByValue("InteriorId", item.InteriorId, newQuantity);
            }
            return;
        }
        public async Task<string> UpdateStatusTransaction(string _id, string status)
        {
            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetByFilterAsync(a => a.TransactionId.Equals(_id) && (a.TransactionStatus.Equals("Pending") || a.TransactionStatus.Equals("Processing")));

            if (item.Any())
            {
                Transaction newItem = _mapper.Map<Transaction>(item);

                if (item.FirstOrDefault().TransactionStatus.Equals("Processing")) newItem.RemainPrice = 0;
                else newItem.RemainPrice = item.FirstOrDefault().RemainPrice;

                if (item.FirstOrDefault().TransactionStatus.Equals("Pending")) newItem.ExpiredDate = DateTime.Now.AddMonths(1);
                if (item.FirstOrDefault().TransactionStatus.Equals("Processing")) newItem.ExpiredDate = DateTime.Now.AddYears(1);

                newItem.TransactionId = _id;
                newItem.Email = item.FirstOrDefault().Email;
                newItem.TotalPrice = item.FirstOrDefault().TotalPrice;
                newItem.TransactionDetail = item.FirstOrDefault().TransactionDetail;
                newItem.CreatedAt = item.FirstOrDefault().CreatedAt;
                newItem.RequestId = item.FirstOrDefault().RequestId;
                newItem.UpdatedAt = DateTime.Now;
                newItem.TransactionStatus = status;
                await _unit.TransactionRepo.UpdateItemByValue("TransactionId", _id, newItem);
                return "Update Successful";
            }
            return "Item does not exist";
        }
        public async Task<int> CalculateTotalPrice(AddCartView[] cartViews)
        {
            int TotalPrice = 0;
            //// Payment cần truyền tổng giá , Add Pending Transaction vào database -> Front end gọi api kèm TransactionView, front end phải gửi tổng giá, account id
            foreach (AddCartView cartView in cartViews)
            {
                IEnumerable<Interior> item = await _unit.InteriorRepo.GetFieldsByFilterAsync(["Price"], a => a.InteriorId.Equals(cartView.InteriorId));
                TotalPrice = TotalPrice + item.FirstOrDefault().Price * cartView.Quantity;
            }
            TotalPrice = (int)Math.Ceiling(TotalPrice + TotalPrice * 0.1 + 100000);
            return TotalPrice;
        }
        public async Task<int> GetRemainPrice(string _id)
        {
            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetFieldsByFilterAsync(["RemainPrice"], a => a.TransactionId.Equals(_id));

            return item.FirstOrDefault().RemainPrice;
        }


        public async Task<string> UpdateTransactionDetail(string _id, AddCartView[] cartviews)
        {

            //IEnumerable<Transaction> items = await _unit.TransactionRepo.GetByFilterAsync(a => a.TransactionId.Equals(_id) && a.TransactionStatus.Equals("Pending"));
            //if (!items.Any()) 
            //{
            //    return "Transaction does not exist";
            //}
            //TransactionDetail[] details = items.FirstOrDefault().TransactionDetail;
            //TransactionDetail[] newDetails = _mapper.Map<TransactionDetail[]>(cartviews);
            //details = details.Concat(newDetails)
            //    .GroupBy(item => item.InteriorId) // Group by product ID
            //    .Select(group => new TransactionDetail { InteriorId = group.Key, Quantity = group.Sum(p => p.Quantity) }) // Sum quantities for each product
            //    .ToArray();

            //int TotalPrice = await CalculateTotalPrice(_mapper.Map<AddCartView[]>(details));

            //Transaction Transaction = _mapper.Map<Transaction>(items);
            //Transaction.TransactionId = _id;
            //Transaction.AccountId = items.FirstOrDefault().AccountId;
            //Transaction.CreatedAt = items.FirstOrDefault().CreatedAt;
            //Transaction.UpdatedAt = DateTime.Now;
            //Transaction.TransactionDetail = details;
            //Transaction.ExpiredDate = DateTime.Now.AddMonths(1);
            //Transaction.TotalPrice = TotalPrice;
            //Transaction.RemainPrice = TotalPrice * 7 / 10;

            //await _unit.TransactionRepo.UpdateItemByValue("TransactionId", _id, Transaction);
            return "Add Item Successful";
        }

        public async Task<(bool, object)> GetTransactionList(string id)
        {
            var getUser = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                g => g.AccountId.Equals(id))).First();
            var getAllTransaction = await _unit.TransactionRepo.GetFieldsByFilterAsync([],
                g => g.Email.Equals(getUser.Email));
            if (getAllTransaction.Any())
            {
                var responses = new List<object>();
                foreach (var item in getAllTransaction)
                {
                    responses.Add(new
                    {
                        TransactionId = item.TransactionId,
                        Email = item.Email,
                        TransactionStatus = item.TransactionStatus
                    });
                }
                return (true, responses);
            }
            return (true, "You don't have any transactions");
        }

        public async Task<(bool, object)> GetTransactionDetail(string transactionId)
        {
            var getTransaction = (await _unit.TransactionRepo.GetFieldsByFilterAsync([],
                g => g.TransactionId.Equals(transactionId))).FirstOrDefault();
            if (getTransaction != null)
            {
                return (true, getTransaction);
            }
            return (false, "TransactionId is not existed");
        }
    }
}
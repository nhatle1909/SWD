using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using ZstdSharp.Unsafe;
using static Repositories.ModelView.CartView;
namespace Services.Service
{
    public class TransactionService : ITransactionService
    {
        string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        string returnUrl = "https://localhost:7220/swagger/index.html";
        string tmnCode = "8G7V30JH";
        string hashSecret = "KBAOJWHYLHYDNOLUXKUAAKMBILURCGPB";

        private readonly IHttpContextAccessor ipget;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        VNPayUtil pay = new VNPayUtil();
        public TransactionService(IUnitOfWork unit, IMapper mapper, IHttpContextAccessor _ipget)
        {
            _unit = unit;
            _mapper = mapper;
            ipget = _ipget;
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
            pay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
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
        public async Task<string> CheckPayment(string url)
        {

            //long vnp_Amount = Convert.ToInt64(Utils.ExtractUrlParam(url, "vnp_Amount")) / 100;
            //long vnpayTranId = Convert.ToInt64(Utils.ExtractUrlParam(url, "vnp_TransactionNo"));

            //string terminalId = Utils.ExtractUrlParam(url, "vnp_TmnCode");
            //string bankCode = Utils.ExtractUrlParam(url, "vnp_BankCode");

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
                    }
                    if (item.FirstOrDefault().TransactionStatus.Equals("Processing"))
                    {
                        await UpdateStatusTransaction(_id, "Success");
                    }
                    return "Checkout Successfull";
                }
                else
                {
                    if (item.FirstOrDefault().TransactionStatus.Equals("Pending"))
                    {
                        await DeleteTransaction(_id);
                    }
                    return "Failed";

                }
            }
            else
            {
                return "Invalid signature";
            }
        }
        public Task<object> GetAllTransaction(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddPendingTransaction(string _id, AddCartView[] cartViews)
        {
            int TotalPrice = await CalculateTotalPrice(cartViews);

            if (!string.IsNullOrEmpty(_id))
            {
                IEnumerable<AccountStatus> item = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["IsAuthenticationEmail"], ass => ass.IsAuthenticationEmail == true && ass.AccountId.Equals(_id));
                if (!item.Any())
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            Transaction Transaction = new Transaction
            {
                TransactionId = ObjectId.GenerateNewId().ToString(),
                TransactionStatus = "Pending",
                AccountId = _id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddMonths(1),
                ContractFile = null,
                TransactionDetail = _mapper.Map<TransactionDetail[]>(cartViews),
                TotalPrice = TotalPrice,
                RemainPrice = TotalPrice * 7 / 10
            };
            await _unit.TransactionRepo.AddOneItem(Transaction);

            return Transaction.TransactionId;
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
            //Transaction.UpdatedAt = DateTime.UtcNow;
            //Transaction.TransactionDetail = details;
            //Transaction.ExpiredDate = DateTime.UtcNow.AddMonths(1);
            //Transaction.TotalPrice = TotalPrice;
            //Transaction.RemainPrice = TotalPrice * 7 / 10;

            //await _unit.TransactionRepo.UpdateItemByValue("TransactionId", _id, Transaction);
            return "Add Item Successful";
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
                if (item.Any() && DateTime.UtcNow > item.FirstOrDefault().ExpiredDate)
                {
                    await _unit.TransactionRepo.RemoveItemByValue("TransactionId", _id);
                }
            }
            return "Delete Succesful";
        }
        //----------------------------------------------------End Interface---------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------------//

        public async Task UpdateInteriorQuantity(string _id)
        {
            IEnumerable<Transaction> items = await _unit.TransactionRepo.GetFieldsByFilterAsync([], a => a.TransactionId.Equals(_id));

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
            IEnumerable<Transaction> item = await _unit.TransactionRepo.GetByFilterAsync(a => a.TransactionId.Equals(_id) && a.TransactionStatus.Equals("Pending") || a.TransactionStatus.Equals("Processing"));

            if (item.Any())
            {
                Transaction newItem = _mapper.Map<Transaction>(item);

                if (item.FirstOrDefault().TransactionStatus.Equals("Processing")) newItem.RemainPrice = 0;
                else newItem.RemainPrice = item.FirstOrDefault().RemainPrice;

                if (item.FirstOrDefault().TransactionStatus.Equals("Pending")) newItem.ExpiredDate = DateTime.UtcNow.AddMonths(1);
                if (item.FirstOrDefault().TransactionStatus.Equals("Processing")) newItem.ExpiredDate = DateTime.UtcNow.AddYears(1);

                newItem.TransactionId = _id;
                newItem.AccountId = item.FirstOrDefault().AccountId;
                newItem.TotalPrice = item.FirstOrDefault().TotalPrice;
                newItem.TransactionDetail = item.FirstOrDefault().TransactionDetail;
                newItem.CreatedAt = item.FirstOrDefault().CreatedAt;

                newItem.UpdatedAt = DateTime.UtcNow;
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

        Task<string> ITransactionService.DeleteExpiredTransaction(string[] _ids)
        {
            throw new NotImplementedException();
        }
    }
}
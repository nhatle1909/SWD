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
namespace Services.Service
{
    public class RequestService : IRequestService
    {
        string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        string returnUrl = "https://localhost:7220/swagger/index.html";
        string tmnCode = "8G7V30JH";
        string hashSecret = "KBAOJWHYLHYDNOLUXKUAAKMBILURCGPB";

        private readonly IHttpContextAccessor ipget;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        VNPayUtil pay = new VNPayUtil();
        public RequestService(IUnitOfWork unit, IMapper mapper, IHttpContextAccessor _ipget)
        {
            _unit = unit;
            _mapper = mapper;
            ipget = _ipget;
        }
        public async Task<string> Payment(string requestId, int TotalPrice)
        {
            //// Payment cần truyền tổng giá , Add Pending request vào database -> Front end gọi api kèm requestView, front end phải gửi tổng giá, account id


            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (TotalPrice * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", requestId); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return paymentUrl;
        }
        public async Task<string> CheckPayment(string url)
        {
            var query = Utils.GetQueryString(url);

            string vnp_SecureHash = Utils.ExtractUrlParam(url, "vnp_SecureHash");
            string _id = Utils.ExtractUrlParam(url, "vnp_TxnRef");
            long vnp_Amount = Convert.ToInt64(Utils.ExtractUrlParam(url, "vnp_Amount")) / 100;
            long vnpayTranId = Convert.ToInt64(Utils.ExtractUrlParam(url, "vnp_TransactionNo"));
            string vnp_ResponseCode = Utils.ExtractUrlParam(url, "vnp_ResponseCode");
            string vnp_TransactionStatus = Utils.ExtractUrlParam(url, "vnp_TransactionStatus");

            string terminalId = Utils.ExtractUrlParam(url, "vnp_TmnCode");
            string bankCode = Utils.ExtractUrlParam(url, "vnp_BankCode");

            bool checkSignature = pay.ValidateSignature(query, vnp_SecureHash, hashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    await UpdateStatusRequest(_id);
                    return "Checkout Successfull";
                }
                else
                {
                    return "Failed";
                }
            }
            else
            {
                return "Invalid signature";
            }
        }
        public Task<object> GetAllRequest(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddPendingRequest(string _id, int totalPrice)
        {

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
            Request request = new Request
            {
                RequestId = ObjectId.GenerateNewId().ToString(),
                RequestStatus = "Pending",
                AccountId = _id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TotalPrice = totalPrice
            };
            await _unit.RequestRepo.AddOneItem(request);
            return request.RequestId;
        }

        public async Task<string> UpdateStatusRequest(string _id)
        {
            IEnumerable<Request> item = await _unit.RequestRepo.GetByFilterAsync(a => a.RequestId.Equals(_id) && a.RequestStatus.Equals("Pending"));
            if (item.Any())
            {
                Request newItem = _mapper.Map<Request>(item);

                newItem.RequestId = _id;
                newItem.AccountId = item.FirstOrDefault().AccountId;
                newItem.TotalPrice = item.FirstOrDefault().TotalPrice;
                newItem.UpdatedAt = DateTime.UtcNow;
                newItem.RequestStatus = "Success";
                await _unit.RequestRepo.UpdateItemByValue("RequestId", _id, newItem);
                return "Update Successful";
            }
            return "Item does not exist";
        }

        public async Task<string> DeleteRequest(string _id)
        {
            //IEnumerable<Request> item = await _repos.GetFieldsByFilterAsync(["_id","RequestStatus"], a=>a.RequestsId.Equals(_id) && a.RequestStatus.Equals("Pending") );
            //if (item.Any()) 
            //{
            //    await _repos.RemoveItemByValue("RequestID",_id);
            //    return "Delete Successful";
            //}
            return "Item does not exist";
        }


    }
}
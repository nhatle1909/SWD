using Repositories.ModelView;

namespace Services.Interface
{
    public interface IRequestService
    {
        public Task<string> Payment(string requestID, PaymentView paymentView);
        public Task<object> GetAllRequest(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingRequest(PaymentView paymentView);
        public Task<string> UpdateStatusRequest(string _id);
        public Task<string> DeleteRequest(string _id);
        public Task<string> CheckPayment(string url);
    }
}

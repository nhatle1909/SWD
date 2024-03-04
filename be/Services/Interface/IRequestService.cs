using Repositories.ModelView;

namespace Services.Interface
{
    public interface IRequestService
    {
        public Task<string> Payment(string requestID, int TotalPrice);
        public Task<object> GetAllRequest(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingRequest(string _id, int totalPrice);
        public Task<string> UpdateStatusRequest(string _id);
        public Task<string> DeleteRequest(string _id);
        public Task<string> CheckPayment(string url);
    }
}
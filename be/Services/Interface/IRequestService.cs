using Repositories.ModelView;
using static Repositories.ModelView.CartView;

namespace Services.Interface
{
    public interface IRequestService
    {
        public Task<string> Payment(string requestID,int price);
        public Task<int> CalculateDeposit(string requestID);
        public Task<int> CalculateTotalPrice(AddCartView[] cartViews);
        public Task<int> GetRemainPrice(string requestID);
        public Task<object> GetAllRequest(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingRequest(string _id, AddCartView[] cartViews);
        public Task<string> UpdateStatusRequest(string _id,string status);
        public Task<string> DeleteRequest(string _id);
        public Task<string> CheckPayment(string url);
    }
}
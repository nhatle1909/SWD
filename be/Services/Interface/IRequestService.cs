using Repositories.ModelView;
using static Repositories.ModelView.CartView;

namespace Services.Interface
{
    public interface IRequestService
    {
        public Task<string> Payment(string requestID, AddCartView[] cartViews);
        public Task<object> GetAllRequest(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingRequest(string _id, AddCartView[] cartViews);
        public Task<string> UpdateStatusRequest(string _id);
        public Task<string> DeleteRequest(string _id);
        public Task<string> CheckPayment(string url);
    }
}
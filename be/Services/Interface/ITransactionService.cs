using Repositories.ModelView;
using static Repositories.ModelView.CartView;

namespace Services.Interface
{
    public interface ITransactionService
    {
        public Task<string> Payment(string ContractID,int price);
        public Task<int> CalculateDeposit(string ContractID);
        public Task<int> CalculateTotalPrice(AddCartView[] cartViews);
        public Task<int> GetRemainPrice(string ContractID);
        public Task<object> GetAllContract(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingContract(string _id, AddCartView[] cartViews);
        public Task<string> UpdateStatusContract(string _id,string status);
        public Task<string> DeleteContract(string _id);
        public Task<string> CheckPayment(string url);
        public Task<string> UpdateContractDetail(string _id, AddCartView[] cartViews);
        public Task<string> DeleteExpiredContract(string[] _ids);
    }
}
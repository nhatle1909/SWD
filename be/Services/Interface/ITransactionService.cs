﻿using Repositories.ModelView;
using static Repositories.ModelView.CartView;

namespace Services.Interface
{
    public interface ITransactionService
    {
        public Task<string> Payment(string TransactionID, int price);
        public Task<int> CalculateDeposit(string TransactionID);
        public Task<int> CalculateTotalPrice(AddCartView[] cartViews);
        public Task<int> GetRemainPrice(string TransactionID);
        public Task<object> GetAllTransaction(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddPendingTransaction(string _id, AddCartView[] cartViews);
        public Task<string> UpdateStatusTransaction(string _id, string status);
        public Task<string> DeleteTransaction(string _id);
        public Task<string> CheckPayment(string url);
        public Task<string> UpdateTransactionDetail(string _id, AddCartView[] cartViews);
        public Task<string> DeleteExpiredTransaction(string[] _ids);
    }
}
using System.Linq.Expressions;

namespace Repository.Repository
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task AddOneItem(T item);
        Task<List<T>> AddManyItem(List<T> items);
        Task RemoveItemByValue(string fieldName, string value);
        Task UpdateItemByValue(string fieldName, string value, T replacement);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filterExpression);

        Task<IEnumerable<T>> GetPagedAsync(int skip, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        Task<long> CountAsync();
        Task<IEnumerable<T>> GetFieldsByFilterAsync(string[] fieldNames, Expression<Func<T, bool>> filter);
    }
}

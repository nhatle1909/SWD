using MongoDB.Driver;
using System.Linq.Expressions;

namespace Repositories.Repository
{
    public interface IRepository<T>
    {
        IMongoCollection<T> Collection { get; }
        Task<List<T>> GetAllAsync();
        Task AddOneItem(T item);
        Task<List<T>> AddManyItem(List<T> items);
        Task RemoveItemByValue(string fieldName, string value);
        Task UpdateItemByValue(string fieldName, string value, T replacement);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filterExpression);

        Task<IEnumerable<T>> PagingAsync(int skip, int pageSize, bool isAsc, string sortField, string? searchValue, List<string> searchFields, List<string> returnFields);
        Task<long> CountAsync();
        Task<IEnumerable<T>> GetFieldsByFilterAsync(string[] fieldNames, Expression<Func<T, bool>> filter);
    }
}

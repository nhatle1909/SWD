using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.Repository;
using System.Linq.Expressions;

namespace Models.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IMongoClient _client;
        private readonly string databasename = "SWD";

        public Repository(IMongoClient client)
        {
            _client = client;
        }

        public IMongoCollection<T> Collection
        {
            get
            {
                return _client.GetDatabase(databasename).GetCollection<T>(typeof(T).Name);
            }
        }
        public async Task AddOneItem(T item)
        {
            await Collection.InsertOneAsync(item);
        }

        public async Task<List<T>> AddManyItem(List<T> items)
        {
            await Collection.InsertManyAsync(items);
            return items;
        }

        public async Task<List<T>> GetAllAsync()
        {
            List<T> itemList = await Collection.Find(_ => true).ToListAsync();
            return itemList;
        }

        public async Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filterExpression)
        {
            return await Collection.Find(filterExpression).ToListAsync();
        }

        public async Task RemoveItemByValue(string fieldName, string value)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            await Collection.DeleteOneAsync(filter);
        }

        public async Task UpdateItemByValue(string fieldName, string value, T replacement)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            await Collection.ReplaceOneAsync(filter, replacement);
        }

        public async Task<IEnumerable<T>> PagingAsync(int skip, int pageSize, bool isAsc, string sortField, string? searchValue, List<string> searchFields, List<string> returnFields)
        {
            var filterBuilder = Builders<T>.Filter.Empty;
            if (searchValue != null)
            {
                var filters = new List<FilterDefinition<T>>();
                foreach (var field in searchFields)
                {
                    var fieldFilter = Builders<T>.Filter.Regex(field, new BsonRegularExpression($".*{searchValue}.*", "i"));
                    filters.Add(fieldFilter);
                }
                filterBuilder = Builders<T>.Filter.Or(filters);
            }

            var sortDefinition = isAsc
                ? Builders<T>.Sort.Ascending(sortField)
                : Builders<T>.Sort.Descending(sortField);

            var query = Collection
                .Find(filterBuilder)
                .Sort(sortDefinition)
                .Skip(skip)
                .Limit(pageSize);

            if (returnFields.Any())
            {
                ProjectionDefinition<T> projection = new BsonDocument();
                foreach (var field in returnFields)
                {
                    projection = projection.Include(field);
                }
                query = query.Project<T>(projection);
            }

            var result = await query.ToListAsync();

            return result;
        }


        public async Task<long> CountAsync()
        {
            return await Collection.CountDocumentsAsync(new BsonDocument());
        }

        public async Task<IEnumerable<T>> GetFieldsByFilterAsync(string[] fieldNames, Expression<Func<T, bool>> filter)
        {
            //chỉ lấy những trường mình muốn lấy thay vì lấy tất cả
            var find = Collection.Find(filter);
            if (fieldNames != null && fieldNames.Length > 0)
            {
                var projection = Builders<T>.Projection.Include(fieldNames[0]);
                for (int i = 1; i < fieldNames.Length; i++)
                {
                    projection = projection.Include(fieldNames[i]);
                }
                find = find.Project<T>(projection);
            }
            return await find.ToListAsync();
        }
    }
}


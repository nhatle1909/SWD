using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.CartView;

namespace Services.Interface
{
    public interface ICartService
    {
        Task<string> AddProductFromCart(string accountId, AddCartView add);
        Task<string> DeleteProductFromCart(string interiorId);
        Task<object> GetAllProductToCart(string accountId);
    }
}

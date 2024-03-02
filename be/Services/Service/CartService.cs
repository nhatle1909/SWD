using AutoMapper;
using Repositories.Model;
using Repositories.Repository;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unit;
        public CartService(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public async Task<string> AddCart(string accountId, string interiorId)
        {
            //Cart cart = new Cart()
            return "";
        }
    }
}

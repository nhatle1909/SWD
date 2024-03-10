using AutoMapper;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.Repository;
using Services.Interface;
using Services.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using static Repositories.ModelView.CartView;

namespace Services.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<string> AddProductFromCart(string accountId, AddCartView add)
        {
            var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync(["InteriorId", "Quantity"],
                            g => g.InteriorId.Equals(add.InteriorId))).FirstOrDefault();
            if (getInterior is not null)
            {
                if (add.Quantity <= getInterior.Quantity)
                {
                    var getInteriorFromcart = (await _unit.CartRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(add.InteriorId))).FirstOrDefault();
                    if (getInteriorFromcart is not null)
                    {
                        var quantity = getInteriorFromcart.Quantity + add.Quantity;
                        if (quantity <= getInterior.Quantity)
                        {
                            getInteriorFromcart.Quantity = quantity;
                            await _unit.CartRepo.UpdateItemByValue("InteriorId", getInteriorFromcart.InteriorId, getInteriorFromcart);
                            return "Add product to cart success";
                        }
                        return "The number of products you want to add to the cart exceeds the number of available products";
                    }
                    Cart cart = _mapper.Map<Cart>(add);
                    cart.AccountId = accountId;
                    await _unit.CartRepo.AddOneItem(cart);
                    return "Add product to cart success";
                }
                return "The number of products you want to add to the cart exceeds the number of available products";
            }
            return "Product is not existed";
        }

        public async Task<string> DeleteProductFromCart(string interiorId)
        {
            var getInteriorFromcart = (await _unit.CartRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(interiorId))).FirstOrDefault();
            if (getInteriorFromcart is not null)
            {
                await _unit.CartRepo.RemoveItemByValue("InteriorId", interiorId);
                return "Delete product to cart success";
            }
            return "Product is not existed";
        }

        public async Task<object> GetAllProductToCart(string accountId)
        {
            var getAllFromCart = await _unit.CartRepo.GetFieldsByFilterAsync([],
                            g => g.AccountId.Equals(accountId));
            if (getAllFromCart.Any())
            {
                var responses = new List<object>();
                foreach (var product in getAllFromCart.ToList())
                {
                    var getProduct = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(product.InteriorId))).FirstOrDefault();
                    if (getProduct is null)
                    {
                        responses.Add($"The Product with id: {product.InteriorId} is no longer in the System");
                        continue;
                    }
                    responses.Add(new
                    {
                        InteriorId = getProduct.InteriorId,
                        Quantity = product.Quantity,
                        InteriorName = getProduct.InteriorName,
                        Price = getProduct.Price,
                        Image = SomeTool.GetImage(Convert.ToBase64String(getProduct.Image))
                    });
                }
                return responses;
            }
            return "You don't have any product in your cart";
        }
    }
}

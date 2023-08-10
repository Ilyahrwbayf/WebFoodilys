using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;

namespace WebFood.Service.CartService
{
    public interface ICartService
    {
        public string ShoppingCartId { get; set; }
        public Task<int> AddToCartAsync(Meal meal);
        public int RemoveFromCart(int id);
        public void EmptyCart();
        public Task<List<Cart>> GetCartItemsAsync();
        public Task<Cart> GetCartItemAsync(int id);
        public Task<int> GetCountAsync();
        public Task<decimal> GetTotalAsync();
        public Task<int> CreateOrderAsync(Order order);
        public void MigrateCart(string userName);

    }
}

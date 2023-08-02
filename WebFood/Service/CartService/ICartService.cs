using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;

namespace WebFood.Service.CartService
{
    public interface ICartService
    {
        public string ShoppingCartId { get; set; }
        public int AddToCart(Meal meal);
        public int RemoveFromCart(int id);
        public void EmptyCart();
        public List<Cart> GetCartItems();
        public Cart GetCartItem(int id);
        public int GetCount();
        public decimal GetTotal();
        public int CreateOrder(Order order);
        public string GetCartId(HttpContext context, ClaimsPrincipal User);
        public void MigrateCart(string userName);

    }
}

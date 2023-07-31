using Microsoft.AspNetCore.Mvc;
using WebFood.Models.Entities;

namespace WebFood.Service.CartService
{
    public interface ICartService
    {
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public ICartService GetCart(Controller controller);
        public void AddToCart(Meal meal);
        public void RemoveFromCart(int id);
        public void EmptyCart();
        public List<Cart> GetCartItems();
        public int GetCount();
        public decimal GetTotal();
        public int CreateOrder(Order order);
        public string GetCartId(HttpContext context);
        public void MigrateCart(string userName);

    }
}

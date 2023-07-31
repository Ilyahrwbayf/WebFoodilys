using Microsoft.AspNetCore.Mvc;
using WebFood.Models;
using WebFood.Models.Entities;

namespace WebFood.Service.CartService
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _db;
        public string ShoppingCartId { get; set; }
        
        public CartService(AppDbContext db)
        {
            _db = db;
            ShoppingCartId = string.Empty;
        }


        public void AddToCart(Meal meal)
        {
            // Get the matching cart and meal instances
            var cartItem = _db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.MealId == meal.Id);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    MealId = meal.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    RestaurantId = meal.RestaurantId,
                    DateCreated = DateTime.Now
                };
                _db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            _db.SaveChanges();
        }

        public int CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void EmptyCart()
        {
            throw new NotImplementedException();
        }

        public ICartService GetCart(Controller controller)
        {
            throw new NotImplementedException();
        }

        public string GetCartId(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetCartItems()
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public decimal GetTotal()
        {
            throw new NotImplementedException();
        }

        public void MigrateCart(string userName)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}

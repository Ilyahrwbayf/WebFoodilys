using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using WebFood.Models;
using WebFood.Models.Entities;

namespace WebFood.Service.CartService
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _db;
        public string ShoppingCartId { get; set; }
        public CartService(AppDbContext db, IHttpContextAccessor context)
        {
            _db = db;
            ShoppingCartId = context.HttpContext.Session.GetString("CardId");
        }

        public async Task<int> AddToCartAsync(Meal meal)
        {
            // Get the matching cart and meal instances
            var cartItem = await _db.Carts.FirstOrDefaultAsync(
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
                await _db.Carts.AddAsync(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            _db.SaveChanges();
            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _db.Carts.FirstOrDefault(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _db.Carts.Remove(cartItem);
                }
                // Save changes
                _db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = _db.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _db.Carts.Remove(cartItem);
            }
            // Save changes
            _db.SaveChanges();
        }
        public async Task<List<Cart>> GetCartItemsAsync()
        {
            return await _db.Carts.Include(c=>c.Meal).Where(
                  cart => cart.CartId == ShoppingCartId).ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            // Get the count of each item in the cart and sum them up
            int? count = await (from cartItems in _db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).SumAsync();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public async Task<decimal> GetTotalAsync()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = await (from cartItems in _db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Meal.Price).SumAsync();

            return total ?? decimal.Zero;
        }
        public async Task<Cart> GetCartItemAsync(int id)
        {
            return await _db.Carts.Where(c => c.RecordId == id).FirstOrDefaultAsync();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItemsAsync();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems.Result)
            {
                var orderDetail = new OrderDetail
                {
                    MealId = item.MealId,
                    OrderId = order.Id,
                    UnitPrice = item.Meal.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Meal.Price);

                await _db.OrderDetails.AddAsync(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.TotalPrice = orderTotal;

            // Save the order
            _db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.Id;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _db.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            _db.SaveChanges();
        }

    }


}


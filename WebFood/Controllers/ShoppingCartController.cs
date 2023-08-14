using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Service.CartService;
using WebFood.Service.MealService;

namespace WebFood.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IDaoMeal _daoMeal;
        public ShoppingCartController(ICartService cartService, IDaoMeal daoMeal)
        {
            _cartService = cartService;
            _daoMeal = daoMeal;
        }

        public IActionResult Cart()
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = _cartService.GetCartItemsAsync().Result,
                CartTotal = _cartService.GetTotalAsync().Result,
            };
            return View(viewModel);
        }

        
        public IActionResult MiniCart()
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = _cartService.GetCartItemsAsync().Result,
                CartTotal = _cartService.GetTotalAsync().Result
            };
            return View(viewModel);
        }

        
        public IActionResult AddToCart(int id)
        {
            Meal meal = _daoMeal.GetAsync(id).Result;
            int itemCount = _cartService.AddToCartAsync(meal).Result;

            // Display the confirmation message
            var results = new ShoppingCartAddViewModel
            {
                Message = $"Блюдо {meal.Name} добавлено в корзину",
                CartTotal = _cartService.GetTotalAsync().Result,
                CartCount = _cartService.GetCountAsync().Result,
                ItemCount = itemCount,
                AddedId = _cartService
                        .GetCartItemsAsync().Result
                        .Where(c=>c.MealId==id)
                        .FirstOrDefault().RecordId
            };
            return Json(results);
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            int mealId = _cartService.GetCartItemAsync(id).Result.MealId;
            string mealName = _daoMeal.GetAsync(mealId).Result.Name;
            int itemCount = _cartService.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = $"Блюдо {mealName} убрано из корзины",
                CartTotal = _cartService.GetTotalAsync().Result,
                CartCount = _cartService.GetCountAsync().Result,
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        public IActionResult CartSummary()
        {
            ViewData["CartCount"] = _cartService.GetCountAsync().Result;
            return PartialView("CartSummary");
        }

        public IActionResult EmptyCart()
        {
            _cartService.EmptyCart();
            TempData["Message"] = "Корзина очищена";

            return RedirectToAction("Cart");
        }

        public IActionResult EmptyCartAjax()
        {
            _cartService.EmptyCart();
            return Ok();
        }

        public IActionResult CheckRestaurantId(int mealId)
        {
            int restaurantId = _daoMeal.GetAsync(mealId).Result.RestaurantId;
            int restaurantIdInCart = _cartService.GetRestaurantId();

            bool correctRestaurant;
            if (restaurantIdInCart == 0)
                    correctRestaurant = true;
            else
                correctRestaurant = (restaurantId == restaurantIdInCart);       
            return Json(correctRestaurant);
        }

    }
}






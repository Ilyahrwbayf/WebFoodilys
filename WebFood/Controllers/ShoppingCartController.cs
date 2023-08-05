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
                CartItems = _cartService.GetCartItems(),
                CartTotal = _cartService.GetTotal()
            };
            return View(viewModel);
        }

        public IActionResult AddToCart(int id)
        {
            Meal meal = _daoMeal.GetAsync(id).Result;
            _cartService.AddToCart(meal);

            return RedirectToAction("Cart");
        }
        //AJAX AddtoCartAJAX
        [HttpPost]
        public IActionResult AddToCartAjax(int id)
        {
            int mealId = _cartService.GetCartItem(id).MealId;
            Meal meal = _daoMeal.GetAsync(mealId).Result;
            int itemCount = _cartService.AddToCart(meal);
            

            // Display the confirmation message
            var results = new ShoppingCartAddViewModel
            {
                Message = "Блюдо " + meal.Name + " добавлено в корзину",
                CartTotal = _cartService.GetTotal(),
                CartCount = _cartService.GetCount(),
                ItemCount = itemCount,
                AddedId = id
            };
            return Json(results);
        }


        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            int mealId = _cartService.GetCartItem(id).MealId;
            string mealName = _daoMeal.GetAsync(mealId).Result.Name;
            int itemCount = _cartService.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "Блюдо " + mealName + " убрано из корзины",
                CartTotal = _cartService.GetTotal(),
                CartCount = _cartService.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        public IActionResult CartSummary()
        {
            ViewData["CartCount"] = _cartService.GetCount();
            return PartialView("CartSummary");
        }

        public IActionResult EmptyCart()
        {
            _cartService.EmptyCart();
            TempData["Message"] = "Корзина очищена";

            return RedirectToAction("Cart");
        }

    }
}

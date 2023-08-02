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
            if (_cartService.ShoppingCartId=="")
            _cartService.ShoppingCartId = SetCardId(HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = _cartService.GetCartItems(),
                CartTotal = _cartService.GetTotal()
            };
            return View(viewModel);
        }

        public IActionResult AddToCart(int id)
        {
            if (_cartService.ShoppingCartId == "")
                _cartService.ShoppingCartId = SetCardId(HttpContext);

            Meal meal = _daoMeal.GetAsync(id).Result;
            _cartService.AddToCart(meal);

            return RedirectToAction("Cart");
        }
        //AJAX AddtoCartAJAX
        [HttpPost]
        public IActionResult AddToCartAjax(int id)
        {
            if (_cartService.ShoppingCartId == "")
                _cartService.ShoppingCartId = SetCardId(HttpContext);

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
            if (_cartService.ShoppingCartId == "")
                _cartService.ShoppingCartId = SetCardId(HttpContext);

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
            if (_cartService.ShoppingCartId == "")
                _cartService.ShoppingCartId = SetCardId(HttpContext);

            ViewData["CartCount"] = _cartService.GetCount();
            return PartialView("CartSummary");
        }


        public string SetCardId(HttpContext context)
        {
            string CartSessionKey = "CardId";
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (User.FindFirstValue(ClaimTypes.Email) != null)
                {
                    context.Session.SetString(CartSessionKey, User.FindFirstValue(ClaimTypes.Email));
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }

    }
}

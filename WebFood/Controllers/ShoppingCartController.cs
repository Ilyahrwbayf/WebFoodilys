using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
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
            Meal addedAlbum = _daoMeal.GetAsync(id).Result;
            _cartService.AddToCart(addedAlbum);

            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            string mealName = _daoMeal.GetAsync(id).Result.Name;
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
    }
}

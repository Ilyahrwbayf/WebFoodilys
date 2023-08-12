using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Service.CartService;
using WebFood.Service.OrderService;
using WebFood.Service.UserService;
using WebFood.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebFood.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private const string promoCode = "FREE";

        private readonly IDaoUser _daoUser;
        private readonly IOrderService _orderSevice;
        private readonly ICartService _cartService;

        public OrderController(IDaoUser daoUser,
                               IOrderService orderService,
                               ICartService cartService)
        {
            _daoUser = daoUser;
            _orderSevice = orderService;
            _cartService = cartService;
        }

        public ActionResult AddressAndPayment()
        {
            if (_cartService.GetCartItemsAsync().Result.Any())
            {
                Profile profile = GetUserProfile();

                Order order = new Order()
                {
                    ProfileId = profile.Id,
                    Profile = profile,
                    RestaurantId = _cartService.GetCartItemsAsync().Result.First().RestaurantId,
                    TotalPrice = _cartService.GetTotalAsync().Result,
                    OrderDate = DateTime.Now,
                    Status = StatusHelper.statuses["Preparing"]
                };

                return View(order);
            }
            else
            {
                TempData["Message"] = "Корзина пуста";
                return RedirectToAction("Cart", "ShoppingCart");
            }

        }

        [HttpPost]
        public ActionResult AddressAndPayment(Order order)
        {

            if (ModelState.IsValid)
            {
                Profile profile = GetUserProfile();
                profile.Name = order.Profile.Name;

                profile.DeliveryAdres = order.Profile.DeliveryAdres;
                profile.Phone = order.Profile.Phone;
                _daoUser.UpdateProfile(profile);

                order.Profile = profile;

                _orderSevice.AddAsync(order);
                _cartService.CreateOrderAsync(order);
            }

            return View(order);
        }

        private Profile GetUserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile profile = _daoUser.GetProfileAsync(userId).Result;
            return profile;
        }
    }
}

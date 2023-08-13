using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
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
        private const string rightPromoCode = "FREE";

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

        public ActionResult OrderInfo()
        {
            if (_cartService.GetCartItemsAsync().Result.Any())
            {
                Profile profile = GetUserProfile();

                return View(profile);
            }
            else
            {
                TempData["Message"] = "Корзина пуста";
                return RedirectToAction("Cart", "ShoppingCart");
            }

        }

        [HttpPost]
        public ActionResult OrderInfo(Profile profile)
        {
            if (ModelState.IsValid)
            {
                Profile profileToUpdate = GetUserProfile();
                profileToUpdate.Name = profile.Name;
                profileToUpdate.DeliveryAdres = profile.DeliveryAdres;
                profileToUpdate.Phone = profile.Phone;

                _daoUser.UpdateProfile(profileToUpdate);

                return RedirectToAction("Payment");
            }

            return View(profile);
        }

        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel paymentView)
        {
            string promoCode = paymentView.PromoCode;

            if (promoCode == rightPromoCode)
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

                _orderSevice.AddAsync(order);
                _cartService.CreateOrderAsync(order);

                ViewBag.Order = $"Заказ номер {order.Id} оплачен и готовиться";
            }
            else
            {
                ViewBag.Message = "Оплата не удалась, попробуйте снова";
            }

            return View(paymentView);
        }


        private Profile GetUserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile profile = _daoUser.GetProfileAsync(userId).Result;
            return profile;
        }
    }
}

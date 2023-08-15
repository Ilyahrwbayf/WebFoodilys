using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Service.CartService;
using WebFood.Service.OrderService;
using WebFood.Service.RestaurantService;
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
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IDaoRestaurant _daoRestaurant;

        public OrderController(IDaoUser daoUser,
                               IOrderService orderService,
                               ICartService cartService,
                               IDaoRestaurant daoRestaurant)
        {
            _daoUser = daoUser;
            _orderService = orderService;
            _cartService = cartService;
            _daoRestaurant = daoRestaurant;
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
                    RestaurantId = _cartService.GetRestaurantId(),
                    TotalPrice = _cartService.GetTotalAsync().Result,
                    OrderDate = DateTime.Now,
                    Status = StatusHelper.statuses["Processed"]
                };

                _orderService.AddAsync(order);
                _cartService.CreateOrderAsync(order);

                ViewBag.Order = $"Заказ номер {order.Id} оплачен и готовиться";
            }
            else
            {
                ViewBag.Message = "Оплата не удалась, попробуйте снова";
            }

            return View(paymentView);
        }

        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult RestaurantOrders(int restaurantId)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(restaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                var orders = _orderService
                            .GetRestaurantOrdersAsync(restaurantId)
                            .Result
                            .OrderByDescending(o => o.OrderDate)
                            .ToList();

                return View(orders);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult RestaurantOrder(int orderId)
        {
            Order order = _orderService.GetOrderAsync(orderId).Result;
            Restaurant restaurant = _daoRestaurant.GetAsync(order.RestaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                Profile profile = _daoUser.GetProfileAsync(order.ProfileId).Result;
                order.Profile = profile;

                var statuses = StatusHelper.statuses;
                SelectList selectList = new SelectList(statuses.OrderBy(x => x.Value),"Value","Value");
                ViewBag.Statuses = selectList;

                return View(order);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult RestaurantOrder(Order order)
        {
            var orderToUpdate = _orderService.GetOrderAsync(order.Id).Result;
            Restaurant restaurant = _daoRestaurant.GetAsync(orderToUpdate.RestaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                Profile profile = _daoUser.GetProfileAsync(orderToUpdate.ProfileId).Result;
                orderToUpdate.Profile = profile;

                var statuses = StatusHelper.statuses;
                SelectList selectList = new SelectList(statuses.OrderBy(x => x.Value),"Value","Value");
                ViewBag.Statuses = selectList;

                orderToUpdate.Status = order.Status;
                _orderService.Update(orderToUpdate);

                return View(orderToUpdate);
            }

            return RedirectToAction("Index", "Home");
        }

        private Profile GetUserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile profile = _daoUser.GetProfileByUserAsync(userId).Result;
            return profile;
        }
    }
}

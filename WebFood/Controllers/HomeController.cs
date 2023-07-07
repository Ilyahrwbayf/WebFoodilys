using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Utility;
using WebPlanner.Models;

namespace WebFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(HomeViewModel model)
        {
            model.Restaurants = _db.Restaurants.ToList();
            model.Categories = _db.Categories.ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM)
        {
            GetRestaurantCategories();
            return View(restaurantVM);
        }

        [HttpPost]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM, [FromForm(Name = "Restaurant.Imageurl")]IFormFile Imageurl)
        {
            var restaurant = restaurantVM.Restaurant;
            var categoryId = restaurantVM.CategoryId;
            GetRestaurantCategories();

            if (restaurant.ManagerId == 0) restaurant.ManagerId = null;

            if (restaurant.ManagerId != null)
            {
                if (_db.Users.Select(u => u.Id == restaurant.ManagerId).Any())
                {
                    AddRestaurantToDb(restaurant, categoryId, Imageurl);
                }
                else
                {
                    ViewBag.Message = "Неверный id пользователя";
                }
            }
            else
            {
                AddRestaurantToDb(restaurant, categoryId, Imageurl);
            }
            return View(restaurantVM);
        }

        private void GetRestaurantCategories()
        {
            var categories = _db.Categories.ToList();
            ViewBag.RestaurantCategories = new SelectList(categories, "Id", "Name");
        }

        private void AddRestaurantToDb(Restaurant restaurant, int categoryId, IFormFile Imageurl)
        {
            
            restaurant.Imageurl = GetImageUrl(Imageurl).Result.ToString();

            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();

            _db.RestaurantType.Add(new RestaurantType(restaurant.Id, categoryId));
            _db.SaveChanges();
            ViewBag.Message = "Ресторан " + restaurant.Name + " добавлен";
        }

        private async Task<string> GetImageUrl(IFormFile Imageurl)
        {
            string url = "";
            try
            {
               url = await FileUploadHelper.Upload(Imageurl);
               
            }
            catch (Exception) {}
            return url;
        }

        [HttpGet]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            return View(typeOfRestaurant);
        }

        [HttpPost]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant, IFormCollection formValues)
        {
            _db.Categories.Add(typeOfRestaurant);
            _db.SaveChanges();
            ViewBag.Message = "Категория " + typeOfRestaurant.Name + " добавлена";
            return View(typeOfRestaurant);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
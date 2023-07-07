using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Utility;
using WebPlanner.Models;
using System.IO;

namespace WebFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _db;
        public RestaurantController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Restaurant(int restaurantId)
        {
           var restaurant = GetRestaurant(restaurantId);
           ViewData["Title"] = restaurant.Name;
           return View(restaurant);
        }

        [HttpGet]
        public IActionResult Edit(int restaurantId)
        {
            var restaurant = GetRestaurant(restaurantId);
            AddRestaurantVM restaurantVM = new AddRestaurantVM();
            restaurantVM.Restaurant = restaurant;
            restaurantVM.CategoryId = GetRestaurantCategory(restaurantId);
            GetRestaurantCategories();
            return View(restaurantVM);
        }

        [HttpPost]
        public IActionResult Edit(AddRestaurantVM restaurantVM)
        {
            _db.Restaurants.Update(restaurantVM.Restaurant);
            _db.SaveChanges();
            ViewBag.Message = "Информация изменена";
            return View(restaurantVM);
        }

        [HttpGet]
        public IActionResult ChangeImage(int restaurantId)
        {
            var restaurant = GetRestaurant(restaurantId);
            return View(restaurant);
        }

        [HttpPost]
        public IActionResult ChangeImage(Restaurant restaurant, [FromForm(Name = "Imageurl")] IFormFile Imageurl)
        {
            restaurant = GetRestaurant(restaurant.Id);

            string filePath = "wwwroot/"+restaurant.Imageurl;


            if (System.IO.File.Exists(filePath)) {
                System.IO.File.Delete(filePath);
            }

            restaurant.Imageurl = GetImageUrl(Imageurl).Result.ToString();

            _db.Restaurants.Update(restaurant);
            _db.SaveChanges();
            ViewBag.Message = "Обложка изменена";
            return View(restaurant);
        }

        private async Task<string> GetImageUrl(IFormFile Imageurl)
        {
            string url = "";
            try
            {
                url = await FileUploadHelper.Upload(Imageurl);

            }
            catch (Exception) { }
            return url;
        }


        private Restaurant GetRestaurant(int restaurantId)
        {
            return _db.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
        }
        private int GetRestaurantCategory(int restaurantId)
        {
            return _db.RestaurantType.FirstOrDefault(rt => rt.RestaurantId == restaurantId).TypeId;
        }
        private void GetRestaurantCategories()
        {
            var categories = _db.Categories.ToList();
            ViewBag.RestaurantCategories = new SelectList(categories, "Id", "Name");
        }
    }
}

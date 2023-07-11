using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Utility;
using WebPlanner.Models;
using System.IO;
using WebFood.Service.RestaurantService;
using WebFood.Service.CategoryService;
using WebFood.Service.TypeOfRestaurantService;
using WebFood.Service.RestaurantTypeService;
using WebFood.Service.UserService;

namespace WebFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IDaoRestaurant _daoRestaurant;
        private readonly IDaoTypeOfRestaurant _daoTypeOfRestaurant;
        private readonly IDaoRestaurantType _daoRestaurantType;
        private readonly IDaoUser _daoUser;
        public RestaurantController(IDaoRestaurant daoRestaurant,
                                    IDaoTypeOfRestaurant daoTypeOfRestaurant,
                                    IDaoRestaurantType daoRestaurantType,
                                    IDaoUser daoUser)
        {
            _daoRestaurant= daoRestaurant;
            _daoTypeOfRestaurant = daoTypeOfRestaurant;
            _daoRestaurantType = daoRestaurantType;
            _daoUser= daoUser;
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
            restaurantVM.CategoryId = GetRestaurantType(restaurantId);
            GetTypesOfRestaurant();
            return View(restaurantVM);
        }

        [HttpPost]
        public IActionResult Edit(AddRestaurantVM restaurantVM)
        {
            var restaurant = restaurantVM.Restaurant;
            if (restaurant.ManagerId == 0) restaurant.ManagerId = null;

            if (ModelState.IsValid)
            {

                if (restaurant.ManagerId != null)
                {
                    if (_daoUser.GetAsync(Convert.ToInt32(restaurant.ManagerId)).Result != null)
                    {
                        _daoRestaurant.Update(restaurantVM.Restaurant);
                        ViewBag.Message = "Информация изменена";
                    }
                    else
                    {
                        ModelState.AddModelError("Restaurant.ManagerId", "Пользователь с таким id не найден");
                    }
                }
                else
                {
                    _daoRestaurant.Update(restaurantVM.Restaurant);
                    ViewBag.Message = "Информация изменена";
                }
            }
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
            if (ModelState.IsValid)
            {
                restaurant = GetRestaurant(restaurant.Id);

                string filePath = "wwwroot/"+restaurant.Imageurl;


                if (System.IO.File.Exists(filePath)) {
                    System.IO.File.Delete(filePath);
                }

                restaurant.Imageurl = GetImageUrl(Imageurl).Result.ToString();

                _daoRestaurant.Update(restaurant);

                ViewBag.Message = "Обложка изменена";
            }
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
            return _daoRestaurant.GetAsync(restaurantId).Result;
        }
        private int GetRestaurantType(int restaurantId)
        {
            return _daoRestaurantType.GetAsync(restaurantId).Result.TypeId;
        }
        private void GetTypesOfRestaurant()
        {
            var categories = _daoTypeOfRestaurant.GetAllAsync().Result;
            ViewBag.RestaurantCategories = new SelectList(categories, "Id", "Name");
        }
    }
}

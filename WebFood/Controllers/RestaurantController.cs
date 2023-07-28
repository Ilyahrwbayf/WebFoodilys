using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Utility;
using WebFood.Models;
using System.IO;
using WebFood.Service.RestaurantService;
using WebFood.Service.CategoryService;
using WebFood.Service.TypeOfRestaurantService;
using WebFood.Service.RestaurantTypeService;
using WebFood.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebFood.Service.MealService;
using WebFood.Service.CategoryOfMealService;

namespace WebFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IDaoRestaurant _daoRestaurant;
        private readonly IDaoTypeOfRestaurant _daoTypeOfRestaurant;
        private readonly IDaoRestaurantType _daoRestaurantType;
        private readonly IDaoUser _daoUser;
        private readonly IDaoMeal _daoMeal;
        private readonly IDaoCategoryOfMeal _daoCategoryOfMeal;

        public RestaurantController(IDaoRestaurant daoRestaurant,
                                    IDaoTypeOfRestaurant daoTypeOfRestaurant,
                                    IDaoRestaurantType daoRestaurantType,
                                    IDaoUser daoUser,
                                    IDaoMeal daoMeal,
                                    IDaoCategoryOfMeal daoCategoryOfMeal
                                    )
        {
            _daoRestaurant= daoRestaurant;
            _daoTypeOfRestaurant = daoTypeOfRestaurant;
            _daoRestaurantType = daoRestaurantType;
            _daoUser= daoUser;
            _daoMeal= daoMeal;
            _daoCategoryOfMeal = daoCategoryOfMeal;
        }
        public IActionResult Restaurant(int restaurantId)
        {
           Restaurant restaurant = GetRestaurant(restaurantId);

           List<Meal> meals = _daoMeal.GetAllAsync(restaurantId).Result;
           ViewBag.Meals = meals;

            List<CategoryOfMeal> menu = _daoCategoryOfMeal.GetAllAsync(restaurantId).Result;
            ViewBag.Menu = menu;

           ViewData["Title"] = restaurant.Name;
           return View(restaurant);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM)
        {
            GetTypesOfRestaurants();
            return View(restaurantVM);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM, [FromForm(Name = "Restaurant.Imageurl")] IFormFile Imageurl)
        {
            var restaurant = restaurantVM.Restaurant;
            var categoryId = restaurantVM.CategoryId;
            GetTypesOfRestaurants();

            if (restaurant.ManagerId == 0) restaurant.ManagerId = null;

            if (ModelState.IsValid)
            {
                if (restaurant.ManagerId != null)
                {
                    if (_daoUser.GetAsync(Convert.ToInt32(restaurant.ManagerId)).Result != null)
                    {
                        AddRestaurantToDb(restaurant, categoryId, Imageurl);
                        ViewBag.Message = "Ресторан " + restaurant.Name + " добавлен";
                    }
                    else
                    {
                        ModelState.AddModelError("Restaurant.ManagerId", "Пользователь с таким id не найден");
                    }
                }
                else
                {
                    AddRestaurantToDb(restaurant, categoryId, Imageurl);
                    ViewBag.Message = "Ресторан " + restaurant.Name + " добавлен";
                }
            }
            return View(restaurantVM);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Edit(int restaurantId)
        {
            var restaurant = GetRestaurant(restaurantId);
            if (IsAdminOrManager(restaurant))
            {
                AddRestaurantVM restaurantVM = new AddRestaurantVM();
                restaurantVM.Restaurant = restaurant;
                restaurantVM.CategoryId = GetRestaurantType(restaurantId);
                GetTypesOfRestaurant();
                return View(restaurantVM);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Edit(AddRestaurantVM restaurantVM)
        {
            var restaurant = restaurantVM.Restaurant;

            if (IsAdminOrManager(restaurant))
            {
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
                GetTypesOfRestaurant();
                return View(restaurantVM);

            }
            else
                return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult ChangeImage(int restaurantId)
        {
            var restaurant = GetRestaurant(restaurantId);
            if (IsAdminOrManager(restaurant))
            {
                return View(restaurant);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult ChangeImage(Restaurant restaurant, [FromForm(Name = "Imageurl")] IFormFile Imageurl)
        {
            restaurant = GetRestaurant(restaurant.Id);

            if (IsAdminOrManager(restaurant))
            {
                if (ModelState.IsValid)
                {

                    string filePath = "wwwroot/" + restaurant.Imageurl;


                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    restaurant.Imageurl = GetImageUrl(Imageurl).Result.ToString();

                    _daoRestaurant.Update(restaurant);

                    ViewBag.Message = "Обложка изменена";
                }
                return View(restaurant);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int restaurantId)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(restaurantId).Result;
            _daoRestaurant.Delete(restaurant);
            TempData["Message"] = $"Ресторан {restaurant.Name} удален";
            return RedirectToAction("Index", "Home");
        }



                                            // HELP METHODS

        private bool IsAdminOrManager(Restaurant restaurant)
        {
            return (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                        || User.IsInRole("Administrator"));
        }
        private void GetTypesOfRestaurants()
        {
            var categories = _daoTypeOfRestaurant.GetAllAsync().Result;
            ViewBag.RestaurantCategories = new SelectList(categories, "Id", "Name");
        }

        private void AddRestaurantToDb(Restaurant restaurant, int categoryId, IFormFile Imageurl)
        {

            restaurant.Imageurl = GetImageUrl(Imageurl).Result.ToString();

            _daoRestaurant.AddAsync(restaurant);

            _daoRestaurantType.AddAsync(new RestaurantType(restaurant.Id, categoryId));
            
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Service.CategoryOfMealService;
using WebFood.Service.MealService;
using WebFood.Service.RestaurantService;
using WebFood.Utility;

namespace WebFood.Controllers
{
    public class MealController : Controller
    {
        private readonly IDaoMeal _daoMeal;
        private readonly IDaoRestaurant _daoRestaurant;
        private readonly IDaoCategoryOfMeal _daoCategoryOfMeal;

        public MealController(IDaoMeal daoMeal, IDaoRestaurant daoRestaurant, IDaoCategoryOfMeal daoCategoryOfMeal)
        {
            _daoMeal = daoMeal;     
            _daoRestaurant = daoRestaurant;
            _daoCategoryOfMeal = daoCategoryOfMeal;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult AddMeal(int restaurantId)
        {
            Restaurant restaurant = GetRestaurant(restaurantId);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                                    || User.IsInRole("Administrator"))
            {
                MealViewModel viewModel = new MealViewModel();
                viewModel.Meal.RestaurantId = restaurantId;
                viewModel.Categories = GetCategoriesOfMeal(restaurantId);

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult AddMeal(MealViewModel viewModel, [FromForm(Name = "Meal.ImageUrl")] IFormFile Imageurl)
        {
            Restaurant restaurant = GetRestaurant(viewModel.Meal.RestaurantId);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                                    || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    AddMealToDb(viewModel.Meal, viewModel.CategoryId, Imageurl);
                }

                viewModel.Categories = GetCategoriesOfMeal(viewModel.Meal.RestaurantId);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult EditMeal(int mealId)
        {
            Meal meal = _daoMeal.GetAsync(mealId).Result;
            Restaurant restaurant = GetRestaurant(meal.RestaurantId);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                                    || User.IsInRole("Administrator"))
            {
                MealViewModel viewModel = new MealViewModel();
                viewModel.Meal = meal;
                viewModel.Categories = GetCategoriesOfMeal(meal.RestaurantId);

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult EditMeal(MealViewModel viewModel)
        {
            Restaurant restaurant = GetRestaurant(viewModel.Meal.RestaurantId);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                                    || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    _daoMeal.Update(viewModel.Meal);
                    ViewBag.Message = "Блюдо " + viewModel.Meal.Name + " изменено";
                }

                viewModel.Categories = GetCategoriesOfMeal(viewModel.Meal.RestaurantId);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Delete(int mealId, int restaurantId)
        {
            Meal meal = _daoMeal.GetAsync(mealId).Result;
            _daoMeal.Delete(meal);
            TempData["Message"] = $"Блюдо {meal.Name} удалено";
            return RedirectToAction("Restaurant", "Restaurant", new {restaurantId = restaurantId});
        }

        //  HELP METHODS

        private void AddMealToDb(Meal meal, int categoryId, IFormFile Imageurl)
        {

            meal.ImageUrl = GetImageUrl(Imageurl).Result.ToString();

            _daoMeal.AddAsync(meal);

            ViewBag.Message = "Блюдо " + meal.Name + " добавлен";
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

        private SelectList GetCategoriesOfMeal(int restaurantId)
        {
            
            List<CategoryOfMeal> categoriesDb = _daoCategoryOfMeal.GetAllAsync(restaurantId).Result;
            List<CategoryOfMeal> categories = new List<CategoryOfMeal>();
            categories.Add(new CategoryOfMeal() { Id = 0,Name="Нет" });

            if (categoriesDb != null)
            {
                categories.AddRange(categoriesDb);
            }

            return new SelectList(categories, "Id", "Name");
        }
    }
}

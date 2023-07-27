using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Service.MealService;
using WebFood.Service.RestaurantService;
using WebFood.Utility;

namespace WebFood.Controllers
{
    public class MealController : Controller
    {
        private readonly IDaoMeal _daoMeal;
        private readonly IDaoRestaurant _daoRestaurant;

        public MealController(IDaoMeal daoMeal, IDaoRestaurant daoRestaurant)
        {
            _daoMeal = daoMeal;     
            _daoRestaurant = daoRestaurant;
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
                viewModel.RestaurantId = restaurantId;

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
            Restaurant restaurant = GetRestaurant(viewModel.RestaurantId);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                                    || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    AddMealToDb(viewModel.Meal, viewModel.CategoryId, Imageurl);
                }

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        private void AddMealToDb(Meal meal, int categoryId, IFormFile Imageurl)
        {

            meal.ImageUrl = GetImageUrl(Imageurl).Result.ToString();

            _daoMeal.AddAsync(meal);

            //_daoRestaurantType.AddAsync(new RestaurantType(restaurant.Id, categoryId));
            ViewBag.Message = "Блюдо " + meal.Name + " добавлен";
        }



                                            //  HELP METHODS

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
    }
}

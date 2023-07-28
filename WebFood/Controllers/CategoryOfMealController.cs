using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Service.CategoryOfMealService;
using WebFood.Service.RestaurantService;

namespace WebFood.Controllers
{
    public class CategoryOfMealController : Controller
    {
        private readonly IDaoCategoryOfMeal _daoCategoryOfMeal;
        private readonly IDaoRestaurant _daoRestaurant;

        public CategoryOfMealController(IDaoCategoryOfMeal daoCategoryOfMeal, IDaoRestaurant daoRestaurant)
        {
            _daoCategoryOfMeal = daoCategoryOfMeal;
            _daoRestaurant = daoRestaurant;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult AddCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = GetRestaurant(restaurantId);
            if (IsAdminOrManager(restaurant))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId }; 
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult AddCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = GetRestaurant(category.RestaurantId);
            if (IsAdminOrManager(restaurant))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.AddAsync(category);
                    ViewBag.Message = "Категория " + category.Name + " добавлена";
                }


                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult EditCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = GetRestaurant(restaurantId);
            if (IsAdminOrManager(restaurant))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId };
                ViewBag.Categories = GetCategoriesOfMeal(restaurantId);
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult EditCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = GetRestaurant(category.RestaurantId);
            if (IsAdminOrManager(restaurant))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.Update(category);
                    ViewBag.Message = "Категория изменена на " + category.Name;
                }
                ViewBag.Categories = GetCategoriesOfMeal(restaurant.Id);
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult DeleteCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = GetRestaurant(restaurantId);
            if (IsAdminOrManager(restaurant))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId };
                ViewBag.Categories = GetCategoriesOfMeal(restaurantId);
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult DeleteCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = GetRestaurant(category.RestaurantId);
            if (IsAdminOrManager(restaurant))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.Delete(category);
                    ViewBag.Message = "Категория " + category.Name + " удалена";
                }
                ViewBag.Categories = GetCategoriesOfMeal(category.RestaurantId);
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        // HELP METHODS

        private Restaurant GetRestaurant(int restaurantId)
        {
            return _daoRestaurant.GetAsync(restaurantId).Result;
        }

        private bool IsAdminOrManager(Restaurant restaurant)
        {
            return (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                        || User.IsInRole("Administrator"));
        }

        private SelectList GetCategoriesOfMeal(int restaurantId)
        {
            List<CategoryOfMeal> categories = _daoCategoryOfMeal.GetAllAsync(restaurantId).Result;
            return new SelectList(categories, "Id", "Name");
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Service.CategoryOfMealService;
using WebFood.Service.RestaurantService;
using WebFood.Utility;

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
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult AddCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(restaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant,User))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId }; 
                return View(category);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult AddCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(category.RestaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.AddAsync(category);
                    ViewBag.Message = "Категория " + category.Name + " добавлена";
                }

                return View(category);
            }
            
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult EditCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(restaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId };
                ViewBag.Categories = GetCategoriesOfMeal(restaurantId);
                return View(category);
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult EditCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(category.RestaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.Update(category);
                    ViewBag.Message = "Категория изменена на " + category.Name;
                }
                ViewBag.Categories = GetCategoriesOfMeal(restaurant.Id);
                return View(category);
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult DeleteCategoryOfMeal(int restaurantId)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(restaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                CategoryOfMeal category = new CategoryOfMeal() { RestaurantId = restaurantId };
                ViewBag.Categories = GetCategoriesOfMeal(restaurantId);
                return View(category);
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}, {Roles.Manager}")]
        public IActionResult DeleteCategoryOfMeal(CategoryOfMeal category)
        {
            Restaurant restaurant = _daoRestaurant.GetAsync(category.RestaurantId).Result;
            if (AcessChecker.IsAdminOrManager(restaurant, User))
            {
                if (ModelState.IsValid)
                {
                    _daoCategoryOfMeal.Delete(category);
                    ViewBag.Message = "Категория " + category.Name + " удалена";
                }
                ViewBag.Categories = GetCategoriesOfMeal(category.RestaurantId);
                return View(category);
            }
            
            return RedirectToAction("Index", "Home");
        }



                                                // HELP METHODS

        private SelectList GetCategoriesOfMeal(int restaurantId)
        {
            List<CategoryOfMeal> categories = _daoCategoryOfMeal.GetAllAsync(restaurantId).Result;
            return new SelectList(categories, "Id", "Name");
        }

    }
}

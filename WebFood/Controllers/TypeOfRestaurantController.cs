using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WebFood.Models.Entities;
using WebFood.Service.CategoryService;
using WebFood.Utility;

namespace WebFood.Controllers
{
    public class TypeOfRestaurantController : Controller
    {
        private readonly IDaoTypeOfRestaurant _daoTypeOfRestaurant;

        public TypeOfRestaurantController(IDaoTypeOfRestaurant daoTypeOfRestaurant)
        {
            _daoTypeOfRestaurant = daoTypeOfRestaurant;
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            return View(typeOfRestaurant);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant, IFormCollection formValues)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.AddAsync(typeOfRestaurant);
                ViewBag.Message = $"Категория {typeOfRestaurant.Name} добавлена";
            }
            return View(typeOfRestaurant);
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult EditTypeOfRestaurant()
        {
            ViewBag.RestaurantCategories = GetTypesOfRestaurants();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult EditTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.Update(typeOfRestaurant);
                ViewBag.Message = $"Категория изменена на {typeOfRestaurant.Name}";
            }
            ViewBag.RestaurantCategories = GetTypesOfRestaurants();
            return View(typeOfRestaurant);
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult DeleteTypeOfRestaurant()
        {
            ViewBag.RestaurantCategories = GetTypesOfRestaurants();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administator}")]
        public IActionResult DeleteTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.Delete(typeOfRestaurant.Id);
                ViewBag.Message = $"Категория {typeOfRestaurant.Name} удалена";
            }
            ViewBag.RestaurantCategories = GetTypesOfRestaurants();
            return View(typeOfRestaurant);
        }

        private SelectList GetTypesOfRestaurants()
        {
            var categories = _daoTypeOfRestaurant.GetAllAsync().Result;
            return new SelectList(categories, "Id", "Name");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WebFood.Models.Entities;
using WebFood.Service.CategoryService;

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
        [Authorize(Roles = "Administrator")]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            return View(typeOfRestaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant, IFormCollection formValues)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.AddAsync(typeOfRestaurant);
                ViewBag.Message = "Категория " + typeOfRestaurant.Name + " добавлена";
            }
            return View(typeOfRestaurant);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditTypeOfRestaurant()
        {
            GetTypesOfRestaurants();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.Update(typeOfRestaurant);
                ViewBag.Message = "Категория изменена на" + typeOfRestaurant.Name;
            }
            GetTypesOfRestaurants();
            return View(typeOfRestaurant);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteTypeOfRestaurant()
        {
            GetTypesOfRestaurants();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteTypeOfRestaurant(TypeOfRestaurant typeOfRestaurant)
        {
            if (ModelState.IsValid)
            {
                _daoTypeOfRestaurant.Delete(typeOfRestaurant.Id);
                ViewBag.Message = "Категория " + typeOfRestaurant.Name + " удалена";
            }
            GetTypesOfRestaurants();
            return View(typeOfRestaurant);
        }

        private void GetTypesOfRestaurants()
        {
            var categories = _daoTypeOfRestaurant.GetAllAsync().Result;
            ViewBag.RestaurantCategories = new SelectList(categories, "Id", "Name");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Service.CategoryService;
using WebFood.Service.RestaurantService;
using WebFood.Service.RestaurantTypeService;
using WebFood.Service.UserService;
using WebFood.Utility;

namespace WebFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDaoRestaurant _daoRestaurant;
        private readonly IDaoTypeOfRestaurant _daoTypeOfRestaurant;


        public HomeController(IDaoRestaurant daoRestaurant,IDaoTypeOfRestaurant daoTypeOfRestaurant)
        {
            _daoRestaurant = daoRestaurant;
            _daoTypeOfRestaurant = daoTypeOfRestaurant;
        }

        [HttpGet]
        public IActionResult Index(HomeViewModel model)
        {
            model.Restaurants = _daoRestaurant.GetAllAsync().Result;
            model.Types = new SelectList(MakeTypedFilterList(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model, IFormCollection formValues)
        {
            model.Types = new SelectList(MakeTypedFilterList(), "Id", "Name");
            model.Restaurants = _daoRestaurant.GetBySearchAsync(model.TypeId, model.SearchString).Result;
            return View(model);
        }

        public List<TypeOfRestaurant> MakeTypedFilterList()
        {
            List<TypeOfRestaurant> types = new List<TypeOfRestaurant>
            {
                new TypeOfRestaurant() { Id = 0, Name = "Все" }
            };
            types.AddRange(_daoTypeOfRestaurant.GetAllAsync().Result);
            return types;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Administrator")]
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
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
        private readonly ILogger<HomeController> _logger;        
        private readonly IDaoRestaurant _daoRestaurant;
        private readonly IDaoTypeOfRestaurant _daoTypeOfRestaurant;
        private readonly IDaoRestaurantType _daoRestaurantType;
        private readonly IDaoUser _daoUser;

        public HomeController(ILogger<HomeController> logger,
                              IDaoRestaurant daoRestaurant,
                              IDaoTypeOfRestaurant daoTypeOfRestaurant,
                              IDaoRestaurantType daoRestaurantType,
                              IDaoUser daoUser)
        {
            _logger = logger;
            _daoRestaurant = daoRestaurant;
            _daoTypeOfRestaurant = daoTypeOfRestaurant;
            _daoRestaurantType = daoRestaurantType;
            _daoUser = daoUser;
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

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM)
        {
            GetTypesOfRestaurants();
            return View(restaurantVM);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddRestaurant(AddRestaurantVM restaurantVM, [FromForm(Name = "Restaurant.Imageurl")]IFormFile Imageurl)
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
                    }
                    else
                    {
                        ModelState.AddModelError("Restaurant.ManagerId", "Пользователь с таким id не найден");
                    }
                }
                else
                {
                    AddRestaurantToDb(restaurant, categoryId, Imageurl);
                }
            }
            return View(restaurantVM);
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
                ViewBag.Message = "Категория " + typeOfRestaurant.Name+ " удалена";
            }
            GetTypesOfRestaurants();
            return View(typeOfRestaurant);
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
            ViewBag.Message = "Ресторан " + restaurant.Name + " добавлен";
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Service.UserService;
using WebFood.Utility;

namespace WebFood.Controllers
{
    [Authorize(Roles =$"{Roles.Administator},{Roles.Customer}")]
    public class ProfileController : Controller
    {
        private readonly IDaoUser _daoUser;
        public ProfileController(IDaoUser daoUser)
        {
            _daoUser = daoUser;
        }
        public IActionResult Profile()
        {
            Profile profile = GetUserProfile();
            return View(profile);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            Profile profile = GetUserProfile();
            return View(profile);
        }

        [HttpPost]
        public IActionResult EditProfile(Profile profile)
        {
            Profile profileToUpdate = GetUserProfile();
            profileToUpdate.Name = profile.Name;
            profileToUpdate.DeliveryAdres = profile.DeliveryAdres;
            profileToUpdate.Phone = profile.Phone;

            _daoUser.UpdateProfile(profileToUpdate);
            ViewBag.Message = "Профиль изменен";

            return View(profileToUpdate);
        }

        private Profile GetUserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile profile = _daoUser.GetProfileByUserAsync(userId).Result;
            return profile;
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFood.Models.Entities;
using WebFood.Models;
using WebFood.Models.ViewModels;
using WebFood.Service.UserService;
using WebFood.Utility.PasswordHashers;
using WebFood.Service.CartService;

namespace WebFood.Controllers
{
    public class AccessController : Controller
    {
        private readonly IDaoUser _daoUser;
        private readonly ICartService _cartSevice;
        private string CartSessionKey = "CardId";

        public AccessController(IDaoUser daoUser, ICartService cartService)
        {
            _daoUser = daoUser;
            _cartSevice = cartService;
        }

        private void MigrateShoppingCart(string userEmail)
        {
            // Associate shopping cart items with logged-in user
            _cartSevice.MigrateCart(userEmail);
            HttpContext.Session.SetString(CartSessionKey, userEmail);
        }


        public IActionResult Login()
        {
            // проверка на аутентификацию
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //если пользователь найден аутентификация
            User user = _daoUser.GetByEmailAsync(loginViewModel.Email).Result;

            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email не найден");
                }
                else
                {
                    //Hash password
                    PasswordHasherContext passwordHasher = new PasswordHasherContext(new Md5PasswordHasher());
                    loginViewModel.Password = passwordHasher.Hash(loginViewModel.Password);

                    if (user.Password == loginViewModel.Password)
                    {
                        string role = _daoUser.GetUserRole(user.Id).Result;
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, loginViewModel.Email),
                            new Claim(ClaimTypes.Role, role)
                        };

                        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {

                            AllowRefresh = true,
                            IsPersistent = loginViewModel.KeepLoggedIn
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity), properties);


                        MigrateShoppingCart(loginViewModel.Email);


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Не верный пароль");
                    }
                }

            }

            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult Register(User user)
        {
            var existedUser = _daoUser.GetByEmailAsync(user.Email).Result;

            if (ModelState.IsValid)
            {
                if (existedUser == null)
                {
                    _daoUser.AddAsync(user);
                    ViewBag.Message = "Вы успешно зарегестрированы! Выполните вход, используя ваш email и пароль";
                }
                else
                {
                    ModelState.AddModelError("Email", "Уже есть пользователь с таким email");
                };
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            Guid tempCartId = Guid.NewGuid();
            HttpContext.Session.SetString(CartSessionKey, tempCartId.ToString());

            return RedirectToAction("Index", "Home");
        }

    }
}

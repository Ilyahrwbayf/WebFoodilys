using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;
using WebFood.Service.CategoryService;
using WebFood.Service.RestaurantService;
using WebFood.Service.RestaurantTypeService;
using WebFood.Service.TypeOfRestaurantService;
using WebFood.Service.UserService;
using WebFood.Utility;
using WebFood.Models;
using WebFood.Service.MealService;
using WebFood.Service.CategoryOfMealService;
using WebFood.Service.CartService;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using WebFood.Service.OrderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();  

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlServer"),
        new MySqlServerVersion(new Version(8, 0, 31)));
});
builder.Services.AddAuthentication("Cookies").AddCookie(options =>
{
    options.LoginPath = "/Access/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(168); // неделя
});


builder.Services.AddTransient<IDaoRestaurant, DaoRestaurant>();
builder.Services.AddTransient<IDaoTypeOfRestaurant, DaoTypeOfRestaurant>();
builder.Services.AddTransient<IDaoRestaurantType, DaoRestaurantType>();
builder.Services.AddTransient<IDaoUser, DaoUser>();
builder.Services.AddTransient<IDaoMeal, DaoMeal>();
builder.Services.AddTransient<IDaoCategoryOfMeal, DaoCategoryOfMeal>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();


DbInitializer.Initilize(app);

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSession();

app.Use(async (context, next) =>
{
    string CartSessionKey = "CardId";
    if (context.Session.GetString(CartSessionKey) == null)
    {
        if (context.User.FindFirstValue(ClaimTypes.Email) != null)
        {
            context.Session.SetString(CartSessionKey, context.User.FindFirstValue(ClaimTypes.Email));

        }
        else
        {
            // Generate a new random GUID using System.Guid class
            Guid tempCartId = Guid.NewGuid();
            // Send tempCartId back to client as a cookie
            context.Session.SetString(CartSessionKey, tempCartId.ToString());
        }
    }

    await next.Invoke();
});

app.Run();
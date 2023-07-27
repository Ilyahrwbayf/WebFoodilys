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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


var app = builder.Build();

DbInitializer.Initilize(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
    

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

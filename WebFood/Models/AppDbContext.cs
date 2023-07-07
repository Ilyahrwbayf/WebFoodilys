using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;

namespace WebPlanner.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<TypeOfRestaurant> Categories { get; set; }
        public DbSet<RestaurantType> RestaurantType { get;set; }
        public DbSet<CategoryOfMeal> CategoriesOfMeals { get;set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderMeal> OrderMeal { get; set; }
        
    }
}

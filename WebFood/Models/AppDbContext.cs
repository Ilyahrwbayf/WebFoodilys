using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;
using WebFood.Utility;

namespace WebFood.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<TypeOfRestaurant> TypesOfRestaurants { get; set; }
        public DbSet<RestaurantType> RestaurantType { get; set; }
        public DbSet<CategoryOfMeal> CategoriesOfMeals { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        
    }
}

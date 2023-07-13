using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;

namespace WebPlanner.Models
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
        public DbSet<OrderMeal> OrderMeal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { Id=1, Email="admin@admin.com", Password="admin"},
                new User() { Id=2, Email="Managment@KFSmail.com", Password = "KFS"}
                );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole() { Id=1, Name = "Customer"},
                new UserRole() { Id=2, Name = "Manager"},
                new UserRole() { Id=3, Name = "Administrator"}                
                );
            modelBuilder.Entity<Profile>().HasData(
            new Profile() {Id=1,Name="Admin",RoleId=3,UserId=1},
            new Profile() { Id=2,Name="Alexandr",RoleId=2,UserId=2}
            );
        }
        
    }
}

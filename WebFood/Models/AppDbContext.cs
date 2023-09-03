using Microsoft.EntityFrameworkCore;
using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////              Convert DateTime to DateTimeKind
            ///

            modelBuilder.Entity<Cart>()
                .Property(p => p.DateCreated)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );

            modelBuilder.Entity<Meal>()
                .Property(p => p.CreatedDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );

            modelBuilder.Entity<Order>()
                .Property(p => p.OrderDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
        }


    }
}

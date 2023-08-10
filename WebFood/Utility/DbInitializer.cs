﻿using WebFood.Models.Entities;
using WebFood.Models;
using WebFood.Utility.PasswordHashers;

namespace WebFood.Utility
{
    public static class DbInitializer
    {
        public static void Initilize(IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var db = services.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!db.TypesOfRestaurants.Any())
            {
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 1, Name = "Фастфуд" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 2, Name = "Бургеры" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 3, Name = "Суши" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 4, Name = "Пицца" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 5, Name = "Шаурма" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 6, Name = "Грузинская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 7, Name = "Русская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 8, Name = "Вьетнамская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 9, Name = "Японская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Id = 10, Name = "Итальянская" });
                db.SaveChanges();
            }

            if (!db.UserRoles.Any())
            {
                db.UserRoles.Add(new UserRole() { Id = 1, Name = $"{Roles.Customer}" });
                db.UserRoles.Add(new UserRole() { Id = 2, Name = $"{Roles.Manager}" });
                db.UserRoles.Add(new UserRole() { Id = 3, Name = $"{Roles.Administator}" });
                db.SaveChanges();
            }

            if (!db.Users.Any())
            {
                PasswordHasherContext passwordHasher = new PasswordHasherContext(new Md5PasswordHasher());
                db.Users.Add(new User() { Id = 1, Email = "admin@admin.com", Password = passwordHasher.Hash("admin") });
                db.Users.Add(new User() { Id = 2, Email = "Managment@KFSmail.com", Password = passwordHasher.Hash("123") });
                db.Users.Add(new User() { Id = 3, Email = "Tester@mail.com", Password = passwordHasher.Hash("123") });
                db.SaveChanges();
            }

            if (!db.Profiles.Any())
            {
                db.Profiles.Add(new Profile() { Id = 1, Name = "Admin", RoleId = 3, UserId = 1 });
                db.Profiles.Add(new Profile() { Id = 2, Name = "Victor", RoleId = 2, UserId = 2 });
                db.Profiles.Add(new Profile() { Id = 3, Name = "Alexandr", RoleId = 1, UserId = 3 });
                db.SaveChanges();
            }


            if (!db.Restaurants.Any())
            {
                db.Restaurants.Add(new Restaurant() { Id = 1, Name = "KFS", Imageurl = "/uploads/restaurant.jpg", ManagerId = 2 });
                db.Restaurants.Add(new Restaurant() { Id = 2, Name = "Burger Queen", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 3, Name = "Highway", Imageurl = "/uploads/restaurant.jpg"});
                db.Restaurants.Add(new Restaurant() { Id = 4, Name = "Tomato", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 5, Name = "We Love Shaurma", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 6, Name = "Вкусно", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 7, Name = "Kitsune", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 8, Name = "Party Pizza", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 9, Name = "СушиСуши", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 10, Name = "Точка Плова", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() { Id = 11, Name = "Папа Смит", Imageurl = "/uploads/restaurant.jpg" });
                for (int i = 12; i < 51; i++)
                {
                    db.Restaurants.Add(new Restaurant() { Id = i, Name = "Ресторан", Imageurl = "/uploads/restaurant.jpg" });
                }
                db.SaveChanges();
            }

            if (!db.RestaurantType.Any())
            { 
                db.RestaurantType.Add(new RestaurantType() {Id=1,RestaurantId=1,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {Id=2,RestaurantId=2,TypeId=2 });
                db.RestaurantType.Add(new RestaurantType() {Id=3,RestaurantId=3,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {Id=4,RestaurantId=4,TypeId=10 });
                db.RestaurantType.Add(new RestaurantType() {Id=5,RestaurantId=5,TypeId=5 });
                db.RestaurantType.Add(new RestaurantType() {Id=6,RestaurantId=6,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {Id=7,RestaurantId=7,TypeId=9 });
                db.RestaurantType.Add(new RestaurantType() {Id=8,RestaurantId=8,TypeId=4 });
                db.RestaurantType.Add(new RestaurantType() {Id=9,RestaurantId=9,TypeId=3 });
                db.RestaurantType.Add(new RestaurantType() {Id=10,RestaurantId=10,TypeId=6 });
                db.RestaurantType.Add(new RestaurantType() {Id=11,RestaurantId=10,TypeId=4 });
                var rand = new Random();
                for (int i = 12; i < 51; i++)
                {
                    int typeId = rand.Next(1, 10);
                    db.RestaurantType.Add(new RestaurantType() { Id = i, RestaurantId = i, TypeId = typeId });
                }
                db.SaveChanges();
            }

            if (!db.CategoriesOfMeals.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    db.CategoriesOfMeals.Add(new CategoryOfMeal() { Id=i,Name="Салаты",RestaurantId=i});
                    
                }

                for (int i = 51; i < 101; i++)
                {
                    db.CategoriesOfMeals.Add(new CategoryOfMeal() { Id = i, Name = "Бургеры", RestaurantId = (i-50)});
                }

                db.SaveChanges();
            }

            if (!db.Meals.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        db.Meals.Add(new Meal()
                        {
                            Name = "Салат Цезарь",
                            Price = 399,
                            Calories = 200,
                            Weight = 150,
                            RestaurantId = i,
                            ImageUrl = "/uploads/ceasar.jpg",
                            CategoryId = i,
                            CreatedDate = DateTime.Now
                        });

                        
                    }
                }

                for (int i = 1; i < 51; i++)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        db.Meals.Add(new Meal()
                        {
                            Name = "Бургер",
                            Price = 250,
                            Calories = 500,
                            Weight = 100,
                            RestaurantId = i,
                            ImageUrl = "/uploads/burger.jpg",
                            CategoryId = (i + 50),
                            CreatedDate = DateTime.Now
                        });
                    }
                }

                db.SaveChanges();
            }


        }
    }
}

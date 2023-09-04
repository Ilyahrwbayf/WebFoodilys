using WebFood.Models.Entities;
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
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Фастфуд" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Бургеры" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Суши" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Пицца" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Шаурма" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Грузинская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Русская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Вьетнамская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Японская" });
                db.TypesOfRestaurants.Add(new TypeOfRestaurant() { Name = "Итальянская" });
                db.SaveChanges();
            }

            if (!db.UserRoles.Any())
            {
                db.UserRoles.Add(new UserRole() { Name = $"{Roles.Customer}" });
                db.UserRoles.Add(new UserRole() { Name = $"{Roles.Manager}" });
                db.UserRoles.Add(new UserRole() { Name = $"{Roles.Administator}" });
                db.SaveChanges();
            }

            if (!db.Users.Any())
            {
                PasswordHasherContext passwordHasher = new PasswordHasherContext(new Md5PasswordHasher());
                db.Users.Add(new User() { Email = "admin@admin.com", Password = passwordHasher.Hash("admin") });
                db.Users.Add(new User() { Email = "Managment@KFSmail.com", Password = passwordHasher.Hash("123") });
                db.Users.Add(new User() { Email = "Tester@mail.com", Password = passwordHasher.Hash("123") });
                db.SaveChanges();
            }

            if (!db.Profiles.Any())
            {
                db.Profiles.Add(new Profile() {Name = "Admin", RoleId = 3, UserId = 1 });
                db.Profiles.Add(new Profile() {Name = "Victor", RoleId = 2, UserId = 2 });
                db.Profiles.Add(new Profile() {Name = "Alexandr", RoleId = 1, UserId = 3 });
                db.SaveChanges();
            }


            if (!db.Restaurants.Any())
            {
                db.Restaurants.Add(new Restaurant() {Name = "KFS", Imageurl = "/uploads/restaurant.jpg", ManagerId = 2 });
                db.Restaurants.Add(new Restaurant() {Name = "Burger Queen", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Highway", Imageurl = "/uploads/restaurant.jpg"});
                db.Restaurants.Add(new Restaurant() {Name = "Tomato", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "We Love Shaurma", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Вкусно", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Kitsune", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Party Pizza", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "СушиСуши", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Точка Плова", Imageurl = "/uploads/restaurant.jpg" });
                db.Restaurants.Add(new Restaurant() {Name = "Папа Смит", Imageurl = "/uploads/restaurant.jpg" });
                for (int i = 12; i < 51; i++)
                {
                    db.Restaurants.Add(new Restaurant() {Name = "Ресторан", Imageurl = "/uploads/restaurant.jpg" });
                }
                db.SaveChanges();
            }

            if (!db.RestaurantType.Any())
            { 
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=1,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=2,TypeId=2 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=3,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=4,TypeId=10 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=5,TypeId=5 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=6,TypeId=1 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=7,TypeId=9 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=8,TypeId=4 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=9,TypeId=3 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=10,TypeId=6 });
                db.RestaurantType.Add(new RestaurantType() {RestaurantId=10,TypeId=4 });
                var rand = new Random();
                for (int i = 12; i < 51; i++)
                {
                    int typeId = rand.Next(1, 10);
                    db.RestaurantType.Add(new RestaurantType() {RestaurantId = i, TypeId = typeId });
                }
                db.SaveChanges();
            }

            if (!db.CategoriesOfMeals.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    db.CategoriesOfMeals.Add(new CategoryOfMeal() {Name="Салаты",RestaurantId=i});
                    
                }

                for (int i = 51; i < 101; i++)
                {
                    db.CategoriesOfMeals.Add(new CategoryOfMeal() {Name = "Бургеры", RestaurantId = (i-50)});
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

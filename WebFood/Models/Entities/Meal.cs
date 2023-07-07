namespace WebFood.Models.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set;}
        public Restaurant? Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }
        public CategoryOfMeal? CategoryOfMeal { get; set; }
        public double? Weight { get; set; }
        public double? Calories { get; set; }

        public Meal(string name, string imageUrl, double price, int restaurantId, int categoryId)
        {
            Name = name;
            ImageUrl = imageUrl;
            Price = price;
            RestaurantId = restaurantId;
            CreatedDate = DateTime.Now;
            CategoryId = categoryId;
        }

        public Meal(string name, string? description, string imageUrl, double price, int restaurantId, int categoryId)
            : this (name,imageUrl,price, restaurantId, categoryId)
        {
            Description = description;
        }
        public Meal(string name, string imageUrl, double price, int restaurantId, int categoryId, double weight)
            : this(name, imageUrl, price, restaurantId, categoryId)
        {
            Weight = weight;
        }
        public Meal(string name,string imageUrl, double price, int restaurantId, double calories, int categoryId)
            : this(name, imageUrl, price, restaurantId, categoryId)
        {
            Calories = calories;
        }


    }
}

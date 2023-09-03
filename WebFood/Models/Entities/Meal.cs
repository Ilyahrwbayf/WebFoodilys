using System.ComponentModel.DataAnnotations;

namespace WebFood.Models.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }

        [Range(1,10000,ErrorMessage ="Введите цену")]
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set;}
        public Restaurant? Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }
        public CategoryOfMeal? CategoryOfMeal { get; set; }
        public double Weight { get; set; }
        public double Calories { get; set; }

        public Meal()
        {
            Name = string.Empty;
            ImageUrl = string.Empty;
            Description = string.Empty;
            Weight = 0;
            Calories = 0;
            CreatedDate = DateTime.Now;
        }
    }
}

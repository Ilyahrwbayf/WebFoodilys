using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class MealViewModel
    {
        public Meal Meal { get; set; }
        public List<CategoryOfMeal> Categories { get; set; }
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }

        public MealViewModel()
        {
            Meal = new Meal();
            Categories = new List<CategoryOfMeal>();
            RestaurantId = 0;
            CategoryId = 0;
        }

    }
}

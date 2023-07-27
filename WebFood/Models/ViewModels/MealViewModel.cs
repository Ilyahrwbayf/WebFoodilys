using Microsoft.AspNetCore.Mvc.Rendering;
using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class MealViewModel
    {
        public Meal Meal { get; set; }
        public SelectList? Categories { get; set; }
        public int CategoryId { get; set; }

        public MealViewModel()
        {
            Meal = new Meal();
            CategoryId = 0;
        }

    }
}

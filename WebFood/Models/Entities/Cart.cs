using System.ComponentModel.DataAnnotations;

namespace WebFood.Models.Entities
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }

        // for anonymous shopping
        public string CartId { get; set; }

        public int MealId { get; set; }
        public  Meal? Meal { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

    }
}

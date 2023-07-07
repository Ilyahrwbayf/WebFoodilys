using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class AddRestaurantVM
    {
        public Restaurant Restaurant { get; set; }
        public int CategoryId { get; set; }

        public AddRestaurantVM()
        {
            Restaurant = new Restaurant();
        }
    }
}

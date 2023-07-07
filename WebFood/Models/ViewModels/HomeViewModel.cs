using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Restaurant> Restaurants { get; set; }
        public List <TypeOfRestaurant> Categories { get; set; }

        public HomeViewModel()
        {
            Restaurants = new List<Restaurant>();
            Categories = new List<TypeOfRestaurant>();
        }
    }
}

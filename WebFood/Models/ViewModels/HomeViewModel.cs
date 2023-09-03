using Microsoft.AspNetCore.Mvc.Rendering;
using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Restaurant> Restaurants { get; set; }
        public SelectList? Types { get; set; }
        public int TypeId { get; set; }
        public string SearchString { get; set; }

        public HomeViewModel()
        {
            Restaurants = new List<Restaurant>();
            TypeId = 0;
            SearchString = string.Empty;
        }
    }
}

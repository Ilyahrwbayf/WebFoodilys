using WebFood.Models.Entities;

namespace WebFood.Service.RestaurantTypeService
{
    public interface IDaoRestaurantType
    {
        public Task<List<RestaurantType>> GetAllAsync();
        public void AddAsync(RestaurantType restaurantType);
        public void Update(RestaurantType restaurantType);
        public void DeleteAsync(int id);
        public Task<RestaurantType> GetAsync(int restaurantId);
    }
}

using WebFood.Models.Entities;

namespace WebFood.Service.CategoryService
{
    public interface IDaoTypeOfRestaurant
    {
        public Task<List<TypeOfRestaurant>> GetAllAsync();
        public void AddAsync(TypeOfRestaurant typeOfRestaurant);
        public void Update(TypeOfRestaurant typeOfRestaurant);
        public void Delete(int id);
        public Task<TypeOfRestaurant> GetAsync(int id);
    }
}

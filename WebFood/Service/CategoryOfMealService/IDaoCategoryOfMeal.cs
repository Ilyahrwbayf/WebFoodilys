using WebFood.Models.Entities;

namespace WebFood.Service.CategoryOfMealService
{
    public interface IDaoCategoryOfMeal
    {
        public Task<List<CategoryOfMeal>> GetAllAsync(int restaurantId);
        public Task<CategoryOfMeal> GetAsync(int id);
        public void AddAsync(CategoryOfMeal categoryOfMeal);
        public void Update(CategoryOfMeal categoryOfMeal);
        public void Delete(CategoryOfMeal categoryOfMeal);
    }
}

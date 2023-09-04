using WebFood.Models.Entities;

namespace WebFood.Service.MealService
{
    public interface IDaoMeal
    {
        public Task<List<Meal>> GetAllAsync(int restaurantId);
        public Task<Meal> GetAsync(int id);
        public void AddAsync(Meal meal);
        public void Update(Meal meal);
        public void Delete(Meal meal);
    }
}

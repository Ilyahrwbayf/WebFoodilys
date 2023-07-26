using Microsoft.EntityFrameworkCore;
using WebFood.Models;
using WebFood.Models.Entities;

namespace WebFood.Service.MealService
{
    public class DaoMeal : IDaoMeal
    {
        private readonly AppDbContext _db;
        public DaoMeal(AppDbContext db)
        {
            _db = db;
        }
        public async void AddAsync(Meal meal)
        {
            await _db.Meals.AddAsync(meal);
            _db.SaveChanges();
        }

        public void Delete(Meal meal)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Meal>> GetAllAsync(int restaurantId)
        {
            return await _db.Meals.Where(m => m.RestaurantId == restaurantId).ToListAsync();
        }

        public Task<Meal> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Meal meal)
        {
            throw new NotImplementedException();
        }
    }
}

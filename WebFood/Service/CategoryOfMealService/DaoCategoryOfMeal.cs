using Microsoft.EntityFrameworkCore;
using WebFood.Models;
using WebFood.Models.Entities;

namespace WebFood.Service.CategoryOfMealService
{
    public class DaoCategoryOfMeal : IDaoCategoryOfMeal
    {
        private readonly AppDbContext _db;
        public DaoCategoryOfMeal(AppDbContext db)
        {
            _db = db;
        }
        public async void AddAsync(CategoryOfMeal categoryOfMeal)
        {
            await _db.CategoriesOfMeals.AddAsync(categoryOfMeal);
            _db.SaveChanges();
        }

        public void Delete(CategoryOfMeal categoryOfMeal)
        {
            _db.CategoriesOfMeals.Remove(categoryOfMeal);
            _db.SaveChanges();
        }

        public async Task<List<CategoryOfMeal>> GetAllAsync(int restaurantId)
        {
            return  await _db.CategoriesOfMeals.Where(c=>c.RestaurantId==restaurantId).ToListAsync();
        }

        public Task<CategoryOfMeal> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CategoryOfMeal categoryOfMeal)
        {
            _db.CategoriesOfMeals.Update(categoryOfMeal);
            _db.SaveChanges();
        }
    }
}

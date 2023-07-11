using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;
using WebFood.Service.CategoryService;
using WebPlanner.Models;

namespace WebFood.Service.TypeOfRestaurantService
{
    public class DaoTypeOfRestaurant : IDaoTypeOfRestaurant
    {
        private readonly AppDbContext _db;
        public DaoTypeOfRestaurant(AppDbContext db)
        {
            _db = db;
        }
        public async void AddAsync(TypeOfRestaurant typeOfRestaurant)
        {
            await _db.TypesOfRestaurants.AddAsync(typeOfRestaurant);
            _db.SaveChanges();
        }

        public void DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TypeOfRestaurant>> GetAllAsync()
        {
           return await _db.TypesOfRestaurants.ToListAsync();
        }

        public Task<TypeOfRestaurant> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TypeOfRestaurant typeOfRestaurant)
        {
            throw new NotImplementedException();
        }
    }
}

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

        public void Delete(int id)
        {
            TypeOfRestaurant typeOfRestaurant = GetAsync(id).Result;
            _db.TypesOfRestaurants.Remove(typeOfRestaurant);
            _db.SaveChanges();
        }

        public async Task<List<TypeOfRestaurant>> GetAllAsync()
        {
           return await _db.TypesOfRestaurants.ToListAsync();
        }

        public async Task<TypeOfRestaurant> GetAsync(int id)
        {
            return await _db.TypesOfRestaurants.FindAsync(id);
        }

        public void Update(TypeOfRestaurant typeOfRestaurant)
        {
            _db.TypesOfRestaurants.Update(typeOfRestaurant);
            _db.SaveChanges();
        }
    }
}

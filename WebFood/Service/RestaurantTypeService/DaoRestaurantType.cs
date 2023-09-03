using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebFood.Models.Entities;
using WebFood.Models;

namespace WebFood.Service.RestaurantTypeService
{
    public class DaoRestaurantType : IDaoRestaurantType
    {
        private readonly AppDbContext _db;
        public DaoRestaurantType(AppDbContext db)
        {
            _db = db;
        }
        public async void AddAsync(RestaurantType restaurantType)
        {
            await _db.RestaurantType.AddAsync(restaurantType);
            _db.SaveChanges();
        }

        public void DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RestaurantType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RestaurantType> GetAsync(int restaurantId)
        {
            return await _db.RestaurantType.FirstOrDefaultAsync(rt => rt.RestaurantId == restaurantId);
        }

        public void Update(RestaurantType restaurantType)
        {
            throw new NotImplementedException();
        }
    }
}

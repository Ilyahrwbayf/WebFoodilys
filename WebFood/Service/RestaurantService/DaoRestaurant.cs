using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebFood.Models.Entities;
using WebFood.Models;

namespace WebFood.Service.RestaurantService
{
    public class DaoRestaurant : IDaoRestaurant
    {
        private readonly AppDbContext _db;
        public DaoRestaurant(AppDbContext db)
        {
            _db= db;
        }
        public async void AddAsync(Restaurant restaurant)
        {
            await _db.Restaurants.AddAsync(restaurant);
            _db.SaveChanges();
        }

        public void Delete(Restaurant restaurant)
        {
            string filePath = "wwwroot/" + restaurant.Imageurl;


            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _db.Restaurants.Remove(restaurant);
            _db.SaveChanges();
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await _db.Restaurants.ToListAsync();
        }

        public Task<Restaurant> GetAsync(int id)
        {
           return _db.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }

        public void Update(Restaurant restaurant)
        {
            _db.Restaurants.Update(restaurant);
            _db.SaveChanges();
        }

        public async Task<List<Meal>> GetMealsAsync(int id)
        {
            return await _db.Meals.Where(m=>m.RestaurantId == id).ToListAsync();
        }

        public async Task<List<Restaurant>> GetBySearchAsync(int typeId, string searchString)
        {
            var request = _db.RestaurantType.Include(rt => rt.Restaurant);


            if (typeId != 0 && searchString != null)
            {
                 return await request
                     .Where(rt => rt.TypeId == typeId)
                     .Where(rt => rt.Restaurant.Name.Contains(searchString))
                     .Select(rt => rt.Restaurant)
                     .ToListAsync();
            }
            if (typeId != 0)
            {
                return  await request
                    .Where(rt => rt.TypeId == typeId)
                    .Select(rt => rt.Restaurant)
                    .ToListAsync();
            }
            if (searchString != null)
            {
                return await request
                    .Where(rt => rt.Restaurant.Name.Contains(searchString))
                    .Select(rt => rt.Restaurant)
                    .ToListAsync();
            }

            var restaurants = await request
            .Select(rt => rt.Restaurant)
            .ToListAsync();

            return restaurants;
            
        }
    }
}

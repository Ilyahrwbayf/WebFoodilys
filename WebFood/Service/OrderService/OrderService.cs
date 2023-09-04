using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebFood.Models;
using WebFood.Models.Entities;

namespace WebFood.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        public OrderService(AppDbContext db)
        {
            _db = db;     
        }
        public void AddAsync(Order order)
        {
           _db.Orders.AddAsync(order);
           _db.SaveChanges();
        }

        public void Update(Order order)
        {
            _db.Orders.Update(order);
            _db.SaveChanges();
        }

        public Task<Order> GetOrderAsync(int orderId)
        {
            return _db.Orders.Include(o=>o.OrderDetails)
                        .ThenInclude(od=>od.Meal)
                        .Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public Task<List<Order>> GetRestaurantOrdersAsync(int restaurantId)
        {
            return _db.Orders.Include(o=>o.OrderDetails)
                            .ThenInclude(od => od.Meal)
                            .Where(o=>o.RestaurantId == restaurantId)
                            .ToListAsync();
        }

        public Task<List<Order>> GetUserOrdersAsync(int profileId)
        {
            return _db.Orders.Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Meal)
                            .Where(o => o.ProfileId == profileId)
                            .ToListAsync();
        }
    }
}

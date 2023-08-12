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
    }
}

using System.Linq.Expressions;
using WebFood.Models.Entities;

namespace WebFood.Service.OrderService
{
    public interface IOrderService
    {
        public void AddAsync(Order order);
        public void Update(Order order);
        public Task<Order> GetOrderAsync(int orderId);
        public Task<List<Order>> GetRestaurantOrdersAsync(int restaurantId);
        public Task<List<Order>> GetUserOrdersAsync(int profileId);
    }
}

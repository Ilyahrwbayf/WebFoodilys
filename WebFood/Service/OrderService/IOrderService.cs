using WebFood.Models.Entities;

namespace WebFood.Service.OrderService
{
    public interface IOrderService
    {
        public void AddAsync(Order order);
    }
}

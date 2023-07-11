using WebFood.Models.Entities;

namespace WebFood.Service.UserService
{
    public interface IDaoUser
    {
        public Task<User> GetAsync(int id);
    }
}

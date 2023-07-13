using WebFood.Models.Entities;

namespace WebFood.Service.UserService
{
    public interface IDaoUser
    {
        public Task<User> GetAsync(int id);
        public Task<User> GetByEmailAsync(string email);
        public Task<string> GetUserRole(int userId);
        public void AddAsync(User user);
    }
}

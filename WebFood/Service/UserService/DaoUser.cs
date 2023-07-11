using WebFood.Models.Entities;
using WebPlanner.Models;

namespace WebFood.Service.UserService
{
    public class DaoUser : IDaoUser
    {
        private readonly AppDbContext _db;
        public DaoUser(AppDbContext db)
        {
            _db = db;
        }
        public async Task<User> GetAsync(int id)
        {
           return await _db.Users.FindAsync(id);
        }
    }
}

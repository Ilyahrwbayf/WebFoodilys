using Microsoft.EntityFrameworkCore;
using WebFood.Models.Entities;
using WebFood.Models.ViewModels;
using WebFood.Models;
using WebFood.Utility.PasswordHashers;

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

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async void AddAsync(User user)
        {
            PasswordHasherContext passwordHasher = new PasswordHasherContext(new Md5PasswordHasher());
            user.Password = passwordHasher.Hash(user.Password);
            await _db.Users.AddAsync(user);
            _db.SaveChanges();
            await _db.Profiles.AddAsync(new Profile { UserId = user.Id, RoleId = 1 });
            _db.SaveChanges();
        }

        public async Task<string> GetUserRole(int userId)
        {
            return  _db.Profiles.Include(p=>p.Role).FirstOrDefault(p=>p.UserId == userId).Role.Name;
        }

        public Task<Profile> GetProfileAsync(int userId)
        {
            return _db.Profiles.Where(p=>p.UserId==userId).FirstOrDefaultAsync();
        }

        public void UpdateProfile(Profile profile)
        {
            _db.Profiles.Update(profile);
            _db.SaveChanges();
        }
    }
}

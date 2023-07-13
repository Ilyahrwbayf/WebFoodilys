namespace WebFood.Models.Entities
{
    
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public User()
        {
            Password = string.Empty;
            Email = string.Empty;
        }

        public User(string password, string email)
        {
            Password = password;
            Email = email;
        }

    }
}

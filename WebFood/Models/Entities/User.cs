namespace WebFood.Models.Entities
{
    
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public User(string password)
        {
            Password = password;
        }

        public User(string? email, string password) : this(email)
        {
            Password = password;
        }
    }
}

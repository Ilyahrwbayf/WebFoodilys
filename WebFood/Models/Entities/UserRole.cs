namespace WebFood.Models.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole()
        {
            Name = string.Empty;
        }
        public UserRole(string name)
        {
            Name = name;
        }
    }
}

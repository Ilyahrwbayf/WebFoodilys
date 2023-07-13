namespace WebFood.Models.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public UserRole? Role { get; set; }
        public int RoleId { get; set; }
        public int? UserId { get; set;}
        public User? User { get; set; }
        public string DeliveryAdres { get; set; }

        public Profile()
        {
            Phone = string.Empty;
            DeliveryAdres = string.Empty;
            Name = string.Empty;
        }

        
    }
}

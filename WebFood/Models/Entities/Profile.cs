namespace WebFood.Models.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int? UserId { get; set;}
        public User? User { get; set; }
        public string DeliveryAdres { get; set; }

        public Profile(string phone, string role, string deliveryAdres)
        {
            Phone = phone;
            Role = role;
            DeliveryAdres = deliveryAdres;
        }

        public Profile(string? name, string phone, string role, string deliveryAdres)
            : this(phone, role,deliveryAdres)
        {
            Name = name;
        }

        public Profile(string phone, string role, int? userId, string deliveryAdres)
            : this(phone, role, deliveryAdres)
        {
            UserId = userId;
        }

        public Profile(string? name, string phone, string role, int? userId, string deliveryAdres)
            : this(phone,role,userId,deliveryAdres)
        {
            Name = name;
        }
    }
}

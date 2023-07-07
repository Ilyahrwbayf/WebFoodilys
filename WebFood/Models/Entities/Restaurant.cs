using System.ComponentModel.DataAnnotations;

namespace WebFood.Models.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Imageurl { get; set; }
        public int? ManagerId { get; set; }
        public Profile? Manager { get; set; }

        public Restaurant()
        {
            Name = string.Empty;
            Imageurl= string.Empty;
        }

    }
}

namespace WebFood.Models.Entities
{
    public class TypeOfRestaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TypeOfRestaurant()
        {
            Name = string.Empty;
        }

        public TypeOfRestaurant(string name)
        {
            Name = name;
        }
    }
}

namespace WebFood.Models.Entities
{
    public class CategoryOfMeal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public CategoryOfMeal(string name)
        {
            Name = name;
        }

        public CategoryOfMeal()
        {
            Name = string.Empty;
        }
    }
}

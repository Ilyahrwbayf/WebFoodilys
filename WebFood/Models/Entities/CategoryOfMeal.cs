namespace WebFood.Models.Entities
{
    public class CategoryOfMeal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryOfMeal(string name)
        {
            Name = name;
        }
    }
}

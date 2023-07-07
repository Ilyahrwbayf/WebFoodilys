namespace WebFood.Models.Entities
{
    public class OrderMeal
    {
        // link between order and meal
        public int Id { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public Meal? Meal { get; set; }
        public int MealId { get; set;}

        public OrderMeal(int orderId, int mealId)
        {
            OrderId = orderId;
            MealId = mealId;
        }
    }
}

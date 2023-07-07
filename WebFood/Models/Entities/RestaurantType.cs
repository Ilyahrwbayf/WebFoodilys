namespace WebFood.Models.Entities
{
    public class RestaurantType
    {
        // link between reastaurant and category
        public int Id { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public TypeOfRestaurant? Type { get; set; }
        public int TypeId { get; set; }

        public RestaurantType()
        {
        }

        public RestaurantType(int restaurantId, int typeId)
        {
            RestaurantId = restaurantId;
            TypeId = typeId;
        }
    }
}

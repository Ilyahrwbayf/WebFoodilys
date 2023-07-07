namespace WebFood.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        

        public Order(int profileId, int restaurantId, double totalPrice,
                     DateTime date, string status)
        {
            ProfileId = profileId;
            RestaurantId = restaurantId;
            TotalPrice = totalPrice;
            Date = date;
            Status = status;
        }
    }
}

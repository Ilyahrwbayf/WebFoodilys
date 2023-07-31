namespace WebFood.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        

        public Order(int profileId, int restaurantId, decimal totalPrice,
                     DateTime orderDate, string status)
        {
            ProfileId = profileId;
            RestaurantId = restaurantId;
            TotalPrice = totalPrice;
            OrderDate = orderDate;
            Status = status;
        }
    }
}

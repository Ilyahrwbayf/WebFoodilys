namespace WebFood.Models.ViewModels
{
    public class ShoppingCartAddViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int AddedId { get; set; }
    }
}

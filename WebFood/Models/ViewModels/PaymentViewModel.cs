using WebFood.Models.Entities;

namespace WebFood.Models.ViewModels
{
    public class PaymentViewModel
    {
        public Order Order { get; set; }
        public string PromoCode { get; set; }

        public PaymentViewModel()
        {
            Order = new Order();
            PromoCode = string.Empty;
        }
    }
}

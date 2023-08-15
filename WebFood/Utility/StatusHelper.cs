namespace WebFood.Utility
{
    public static class StatusHelper
    {
        public static readonly Dictionary<string, string> statuses = new Dictionary<string, string>()
        {
            {"Processed","Обрабатывается"},
            {"Preparing","Готовиться"},
            {"InDelivery","Доставляется"},
            {"Delivered","Доставлено"},
            {"Canceled","Отменено" }
        };
    }
}

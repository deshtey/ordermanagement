namespace ordermanagement.domain.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromoCode { get; set; }
        public decimal Value { get; set; }
        public List<CustomerSegment> CustomerSegments { get; set; } = new List<CustomerSegment>();
        public int MinOrders { get; set; }
        public DateTime? MaxSignupDate { get; set; }

        public Discount(string name, string promoCode, decimal value, int minOrders, List<CustomerSegment>? segments, DateTime? maxSignupDate = null)
        {
            Name = name;
            PromoCode = promoCode;
            Value = value;
            MinOrders = minOrders;
            CustomerSegments = segments ?? new List<CustomerSegment>();
            MaxSignupDate = maxSignupDate;
        }
    }
}

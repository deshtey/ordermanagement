using ordermanagement.domain.Enums;

namespace ordermanagement.domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public DateTime DeliveredDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<Discount> AppliedDiscounts { get; set; } = [];
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }

        public Order(int customerId, string shippingAddress, string billingAddress, ICollection<OrderItem> orderItems)
        {
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            OrderItems = orderItems?? [];
            AppliedDiscounts = [];

            CalculateTotalAmount(); 
            ApplyDiscounts(AppliedDiscounts);
        }
        public void MarkAsProcessing()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Error: Order must be Pending to be Processed.");
            Status = OrderStatus.Processing;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Processing)
                throw new InvalidOperationException("Order must be Processing to be Shipped.");
            Status = OrderStatus.Shipped;
            ShippedDate = DateTime.UtcNow;
        }

        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Order must be Shipped to be Delivered.");
            Status = OrderStatus.Delivered;
            DeliveredDate = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Order must be Shipped to be Delivered.");
            Status = OrderStatus.Delivered;
            DeliveredDate = DateTime.UtcNow;
        }

        public void ApplyDiscounts(IEnumerable<Discount> discounts)
        {
            AppliedDiscounts.Clear();
            DiscountAmount = 0;

            foreach (var discount in discounts)
            {
                AppliedDiscounts.Add(discount);
            }

            CalculateDiscountAmount();
            RecalculateFinalAmount();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = OrderItems.Sum(item => item.Quantity * item.UnitPrice);
            RecalculateFinalAmount();
        }

        private void CalculateDiscountAmount()
        {
            if (!AppliedDiscounts.Any())
            {
                DiscountAmount = 0;
                return;
            }

            var bestDiscount = AppliedDiscounts.OrderByDescending(d => d.Value).First();
            DiscountAmount = TotalAmount * bestDiscount.Value;
        }

        private void RecalculateFinalAmount()
        {
            FinalAmount = TotalAmount - DiscountAmount;
            if (FinalAmount < 0) FinalAmount = 0;
        }
    }
}

using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;

namespace ordermanagement.application.Models
{
    public class OrderAnalytics
    {
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalDiscount { get; set; }
        public double AverageFulfillmentTimeHours { get; set; }
        public decimal CancellationRate { get; set; }
    }
    public class OrdersByStatusAnalytics
    {
        public OrderStatus Status { get; set; }
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class OrdersByCustomerSegmentAnalytics
    {
        public CustomerSegment Segment { get; set; }
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageDiscount { get; set; }
        public decimal AverageValue { get; set; }
    }
}

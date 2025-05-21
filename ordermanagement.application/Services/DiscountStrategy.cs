using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;

namespace ordermanagement.application.Services
{

    public class DiscountStrategy : IDiscountStrategy
    {
        public string Name => "Segment Discount";
        public string Description => "Applies a discount based on customer segment";

        public Task<decimal> CalculateDiscountAsync(Order order, Customer customer)
        {
            decimal discountPercentage = customer.Segment switch
            {
                CustomerSegment.NewCustomer => 0.02m,
                CustomerSegment.Regular => 0m,
                CustomerSegment.Premium => 0.05m, 
                CustomerSegment.VIP => 0.10m, 
                _ => 0m
            };

            decimal discountAmount = order.TotalAmount * discountPercentage;
            return Task.FromResult(discountAmount);
        }

        public bool IsApplicable(Order order, Customer customer)
        {
            return customer.Segment != CustomerSegment.Regular; 
        }

    }
    public class OrderHistoryDiscountStrategy : IDiscountStrategy
    {
        private readonly IOrderRepository _orderRepository;

        public OrderHistoryDiscountStrategy(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public string Name => "Loyalty Discount";
        public string Description => "Applies a discount based on order history";

        public async Task<decimal> CalculateDiscountAsync(Order order, Customer customer)
        {
            var previousOrders = await _orderRepository.GetOrdersByCustomerIdAsync(customer.Id);
            int orderCount = previousOrders.Count();

            if ((orderCount + 1) % 5 == 0)
            {
                return order.TotalAmount * 0.15m;
            }

            if (orderCount > 10)
            {
                return order.TotalAmount * 0.05m;
            }

            return 0m;
        }

        public bool IsApplicable(Order order, Customer customer)
        {
            return true; 
        }
    }
}

using ordermanagement.application.Contracts;
using ordermanagement.application.Models;
using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;

namespace ordermanagement.application.Services
{
    public class OrderAnalyticsService : IOrderAnalyticsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderAnalyticsService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<OrderAnalytics> GetOrderAnalyticsAsync(
            DateTimeOffset? startDate = null,
            DateTimeOffset? endDate = null)
        {
            var orders = await _orderRepository.GetAllAsync();

            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value.UtcDateTime);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= endDate.Value.UtcDateTime);
            }

            var ordersList = orders.ToList();

            if (!ordersList.Any())
            {
                return new OrderAnalytics
                {
                    TotalOrders = 0,
                    AverageOrderValue = 0,
                    TotalRevenue = 0,
                    TotalDiscount = 0
                };
            }

            var completedOrders = ordersList
                .Where(o => o.Status == OrderStatus.Delivered)
                .ToList();

            TimeSpan averageFulfillmentTime = TimeSpan.Zero;


            return new OrderAnalytics
            {
                TotalOrders = ordersList.Count,
                AverageOrderValue = ordersList.Average(o => o.TotalAmount),
                TotalRevenue = ordersList.Sum(o => o.FinalAmount),
                TotalDiscount = ordersList.Sum(o => o.DiscountAmount),
                AverageFulfillmentTimeHours = averageFulfillmentTime.TotalHours,
                CancellationRate = (decimal)ordersList.Count(o => o.Status == OrderStatus.Cancelled) / ordersList.Count
            };
        }

        public async Task<IEnumerable<OrdersByStatusAnalytics>> GetOrdersByStatusAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders
                .GroupBy(o => o.Status)
                .Select(g => new OrdersByStatusAnalytics
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(o => o.FinalAmount)
                })
                .OrderByDescending(x => x.Count);
        }

        public async Task<IEnumerable<OrdersByCustomerSegmentAnalytics>> GetOrdersByCustomerSegmentAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();

            var customerDict = customers.ToDictionary(c => c.Id, c => c);

            return orders
                .GroupBy(o => customerDict.ContainsKey(o.CustomerId)
                    ? customerDict[o.CustomerId].Segment
                    : CustomerSegment.Regular)
                .Select(g => new OrdersByCustomerSegmentAnalytics
                {
                    Segment = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(o => o.FinalAmount),
                    AverageDiscount = g.Average(o => o.DiscountAmount),
                    AverageValue = g.Average(o => o.TotalAmount)
                })
                .OrderBy(x => x.Segment);
        }
    }
}
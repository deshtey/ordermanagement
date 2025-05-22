// Add to Program.cs
using Microsoft.Extensions.Caching.Memory;
using ordermanagement.application.Contracts;
using ordermanagement.application.Models;
using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;

namespace ordermanagement.application.Services
{

    public class OptimizedOrderAnalyticsService : IOrderAnalyticsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMemoryCache _cache;
        private const string ANALYTICS_CACHE_KEY = "order_analytics";
        private const int CACHE_EXPIRATION_MINUTES = 15;

        public OptimizedOrderAnalyticsService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IMemoryCache cache)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _cache = cache;
        }

        public async Task<OrderAnalytics> GetOrderAnalyticsAsync(
            DateTimeOffset? startDate = null,
            DateTimeOffset? endDate = null)
        {
            // Create a unique cache key based on parameters
            string cacheKey = $"{ANALYTICS_CACHE_KEY}_{startDate}_{endDate}";

            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out OrderAnalytics cachedAnalytics))
            {
                return cachedAnalytics;
            }

            // If not in cache, calculate
            var analytics = await CalculateAnalyticsAsync(startDate, endDate);

            // Store in cache
            _cache.Set(cacheKey, analytics, TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES));

            return analytics;
        }

        private async Task<OrderAnalytics> CalculateAnalyticsAsync(
            DateTimeOffset? startDate,
            DateTimeOffset? endDate)
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
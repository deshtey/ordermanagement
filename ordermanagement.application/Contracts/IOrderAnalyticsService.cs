using ordermanagement.application.Models;

namespace ordermanagement.application.Contracts
{
    public interface IOrderAnalyticsService
    {
        Task<OrderAnalytics> GetOrderAnalyticsAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
        Task<IEnumerable<OrdersByCustomerSegmentAnalytics>> GetOrdersByCustomerSegmentAsync();
        Task<IEnumerable<OrdersByStatusAnalytics>> GetOrdersByStatusAsync();
    }
}

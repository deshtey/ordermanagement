using Microsoft.AspNetCore.Mvc;
using ordermanagement.application.Contracts;
using ordermanagement.application.Models;

namespace ordermanagement.webapi.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IOrderAnalyticsService _analyticsService;

        public AnalyticsController(IOrderAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Gets overall order analytics
        /// </summary>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <returns>Order analytics data</returns>
        [HttpGet("orders")]
        [ProducesResponseType(typeof(OrderAnalytics), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderAnalytics>> GetOrderAnalytics(
            [FromQuery] DateTimeOffset? startDate = null,
            [FromQuery] DateTimeOffset? endDate = null)
        {
            var analytics = await _analyticsService.GetOrderAnalyticsAsync(startDate, endDate);
            return Ok(analytics);
        }

        /// <summary>
        /// Gets order counts by status
        /// </summary>
        /// <returns>Orders grouped by status</returns>
        [HttpGet("orders-by-status")]
        [ProducesResponseType(typeof(IEnumerable<OrdersByStatusAnalytics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdersByStatusAnalytics>>> GetOrdersByStatus()
        {
            var statusAnalytics = await _analyticsService.GetOrdersByStatusAsync();
            return Ok(statusAnalytics);
        }

        /// <summary>
        /// Gets order analytics by customer segment
        /// </summary>
        /// <returns>Orders grouped by customer segment</returns>
        [HttpGet("orders-by-segment")]
        [ProducesResponseType(typeof(IEnumerable<OrdersByCustomerSegmentAnalytics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdersByCustomerSegmentAnalytics>>> GetOrdersByCustomerSegment()
        {
            var segmentAnalytics = await _analyticsService.GetOrdersByCustomerSegmentAsync();
            return Ok(segmentAnalytics);
        }
    }
}
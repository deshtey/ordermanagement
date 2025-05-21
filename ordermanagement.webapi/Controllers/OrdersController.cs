using Microsoft.AspNetCore.Mvc;
using ordermanagement.application.Contracts;
using ordermanagement.application.Services;
using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;
using ordermanagement.webapi.DTO;

namespace ordermanagement.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderStatusService _orderStatusService;

        public OrdersController(IOrderService orderService, IOrderStatusService orderStatusService)
        {
            _orderService = orderService;
            _orderStatusService = orderStatusService;
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of all orders</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Gets a order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="order">Order data</param>
        /// <returns>Created order with ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Order order24 =new Order(
                2,
                "456 Oak Ave",
                "456 Oak Ave",
                new List<OrderItem>
                {
                    new OrderItem(2, 2, 1, 565.99m),  

                }
            )
            {
                Id = 2,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                Status = OrderStatus.Shipped,
            };
            var createdOrder = await _orderService.CreateOrderAsync(order24);

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="order">Updated order data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("ID in URL must match ID in request body");
            }

            try
            {
                await _orderService.UpdateOrderAsync(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Updates the status of a specific order.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <param name="request">The new status to apply.</param>
        /// <returns>No Content if successful, 400 Bad Request if invalid transition, 404 Not Found if order does not exist.</returns>
        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatus updateOrderStatus)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            try
            {
                await _orderStatusService.UpdateOrderStatusAsync(id, updateOrderStatus.NewStatus);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with ID {id} not found.");
            } 
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "An unexpected error occurred while updating order status.");
            }
        }

    }
}
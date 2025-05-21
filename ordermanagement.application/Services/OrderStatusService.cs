using ordermanagement.application.Contracts;
using ordermanagement.domain.Enums;

namespace ordermanagement.application.Services
{

    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderStatusService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ?? throw new KeyNotFoundException($"Order not found.");
            try
            {
                switch (newStatus)
                {
                    case OrderStatus.Processing:
                        order.MarkAsProcessing();
                        break;
                    case OrderStatus.Shipped:
                        order.MarkAsShipped();
                        break;
                    case OrderStatus.Delivered:
                        order.MarkAsDelivered();
                        break;
                    case OrderStatus.Cancelled:
                        order.Cancel();
                        break;
                    // Add other cases as needed
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newStatus), $"Invalid or unsupported status transition to {newStatus}.");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to update order status: {ex.Message}", ex);
            }

            await _orderRepository.UpdateAsync(order);
        }
    }
}
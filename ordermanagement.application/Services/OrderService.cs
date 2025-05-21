using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;

namespace ordermanagement.application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order, string? promoCode = null);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountService _discountService;
        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IDiscountService discountService)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _discountService = discountService;
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return _orderRepository.GetAllAsync();
        }

        public Task<Order?> GetOrderByIdAsync(int id)
        {
            return _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order, string? promoCode = null)
        {
            
            if (order.Customer == null && order.CustomerId != 0)
            {
                var ss = await _customerRepository.GetAllAsync();
                var FF = ss.Count();
                order.Customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            }
            
            //// Calculate total for order
            ////order.TotalAmount = order.OrderItems.Sum(item => item.UnitPrice * item.Quantity);

            //// Apply discount if promo code is provided
            //if (!string.IsNullOrEmpty(promoCode) && order.Customer != null)
            //{
            //    order.DiscountAmount = await _discountService.CalculateDiscountAmountAsync(order, promoCode);
            //}
            ////apply discount 
            if (order.Customer != null)
            {
                order.DiscountAmount = await _discountService.CalculateDiscountAmountAsync(order);
            }

            //// Set initial status and add to status history
            //order.Status = OrderStatus.Draft;
            //order.StatusHistory.Add(new OrderStatusHistory
            //{
            //    Id = Guid.NewGuid(),
            //    OrderId = order.Id,
            //    Status = OrderStatus.Draft,
            //    Timestamp = DateTime.UtcNow
            //});

            return await _orderRepository.AddAsync(order);
        }


        public Task UpdateOrderAsync(Order order)
        {
            return _orderRepository.UpdateAsync(order);
        }

        public Task DeleteOrderAsync(int id)
        {
            return _orderRepository.DeleteAsync(id);
        }
    }
}
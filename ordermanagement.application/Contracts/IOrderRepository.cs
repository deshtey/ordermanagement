using ordermanagement.domain.Entities;

namespace ordermanagement.application.Contracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersByCustomerIdAsync(int CustomerId);
    }
}
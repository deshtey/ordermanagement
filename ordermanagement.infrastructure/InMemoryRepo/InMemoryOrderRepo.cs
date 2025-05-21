using ordermanagement.domain.Entities;
using ordermanagement.infrastructure.TestData;

namespace ordermanagement.application.Contracts.InMemoryRepo
{
    public class InMemoryOrderRepo : IOrderRepository
    {
        private readonly List<Order> _orders = SeedData.GetOrders().ToList();

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            return Task.FromResult(_orders.AsEnumerable());
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return Task.FromResult(_orders.FirstOrDefault(p => p.Id == id));
        }
        public Task<List<Order>> GetOrdersByCustomerIdAsync(int id)
        {
            return Task.FromResult(_orders.Where(p => p.CustomerId == id).ToList());
        }

        public Task<Order> AddAsync(Order entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = 7;
            }

            _orders.Add(entity);

            return Task.FromResult(entity);
        }

        public Task UpdateAsync(Order entity)
        {
            var existingOrder = _orders.FirstOrDefault(p => p.Id == entity.Id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {entity.Id} not found");
            }

            var index = _orders.IndexOf(existingOrder);
           // entity.UpdatedAt = DateTime.UtcNow;
            _orders[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var order = _orders.FirstOrDefault(p => p.Id == id);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            _orders.Remove(order);

            return Task.CompletedTask;
        }
    }
}
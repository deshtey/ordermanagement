using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;
using ordermanagement.infrastructure.TestData;

namespace ordermanagement.infrastructure.InMemoryRepo
{
    public class InMemoryCustomerRepo : ICustomerRepository
    {
        private readonly List<Customer> _customers = SeedData.GetCustomers().ToList();

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return Task.FromResult(_customers.AsEnumerable());
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            return Task.FromResult(_customers.FirstOrDefault(p => p.Id == id));
        }

        public Task<Customer> AddAsync(Customer entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = 7;
            }

            entity.RegisteredOn = DateTime.UtcNow;
            _customers.Add(entity);

            return Task.FromResult(entity);
        }

        public Task UpdateAsync(Customer entity)
        {
            var existingCustomer = _customers.FirstOrDefault(p => p.Id == entity.Id);

            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {entity.Id} not found");
            }

            var index = _customers.IndexOf(existingCustomer);
           // entity.UpdatedAt = DateTime.UtcNow;
            _customers[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var customer = _customers.FirstOrDefault(p => p.Id == id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            }

            _customers.Remove(customer);

            return Task.CompletedTask;
        }
    }
}
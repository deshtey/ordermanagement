using ordermanagement.application.Contracts;
using ordermanagement.domain.Entities;

namespace ordermanagement.application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(CreateCustomerDto customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return _customerRepository.GetAllAsync();
        }

        public Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return _customerRepository.GetByIdAsync(id);
        }

        public Task<Customer> CreateCustomerAsync(CreateCustomerDto customer)
        {
            var newCustomer = new Customer(
                customer.Name,
                customer.Email,
                customer.Phone,
                customer.Address,
                customer.Segment);
            newCustomer.RegisteredOn = DateTime.Now;
            return _customerRepository.AddAsync(newCustomer);
        }

        public Task UpdateCustomerAsync(Customer customer)
        {
            return _customerRepository.UpdateAsync(customer);
        }

        public Task DeleteCustomerAsync(int id)
        {
            return _customerRepository.DeleteAsync(id);
        }
    }
}
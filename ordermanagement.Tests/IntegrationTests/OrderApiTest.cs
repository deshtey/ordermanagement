using ordermanagement.domain.Entities;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OrderManagement.Tests.IntegrationTests
{
    public class OrderApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        public OrderApiTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateCustomer_ReturnsSuccessAndCustomer()
        {
            // Arrange
            var createCustomerRequest = new CreateCustomerRequest
            {
                Name = $"Test Customer {Guid.NewGuid()}",
                Email = $"test{Guid.NewGuid()}@example.com",
                Phone = "555-123-4567",
                Address = "123 Test St",
                Segment = CustomerSegment.NewCustomer,
                SignupDate = DateTime.UtcNow.AddDays(-60)
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/customers", createCustomerRequest);
            var fff = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); 
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            Assert.NotNull(customer);
            Assert.Equal(createCustomerRequest.Name, customer.Name);
            Assert.Equal(createCustomerRequest.Email, customer.Email);
        }

        private async Task<Customer> CreateTestCustomer(CustomerSegment segment)
        {
            // Arrange
            var createCustomerRequest = new CreateCustomerRequest
            {
                Name = $"Test Customer {Guid.NewGuid()}",
                Email = $"test{Guid.NewGuid()}@example.com",
                Phone = "555-123-4567",
                Address = "123 Test St",
                Segment = segment,
                SignupDate = DateTime.UtcNow.AddDays(-60)
            };

            try
            {
                var response = await _client.PostAsJsonAsync("api/customers", createCustomerRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"HTTP Error: {response.StatusCode}, Content: {errorContent}");
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Customer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during customer creation: {ex.Message}");
                throw;
            }
        }
    }

    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public CustomerSegment Segment { get; set; }
        public DateTime SignupDate { get; set; }
    }
}
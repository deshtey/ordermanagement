using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace OrderManagement.Tests.IntegrationTests
{
    public class OrderApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public OrderApiTests(CustomWebApplicationFactory<Program> factory)
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
                PhoneNumber = "555-123-4567",
                Address = "123 Test St",
                Segment = CustomerSegment.NewCustomer,
                SignupDate = DateTime.UtcNow.AddDays(-60)
            };

            // Act
            // Remove the leading slash to avoid double slashes if base URL already ends with one
            var response = await _client.PostAsJsonAsync("api/customers", createCustomerRequest);

            // Assert
            response.EnsureSuccessStatusCode(); // Uncomment this to ensure the request succeeded
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            Assert.NotNull(customer);
            Assert.Equal(createCustomerRequest.Name, customer.Name);
            Assert.Equal(createCustomerRequest.Email, customer.Email);
            // Add more assertions as needed
        }

        private async Task<Customer> CreateTestCustomer(CustomerSegment segment)
        {
            // Arrange
            var createCustomerRequest = new CreateCustomerRequest
            {
                Name = $"Test Customer {Guid.NewGuid()}",
                Email = $"test{Guid.NewGuid()}@example.com",
                PhoneNumber = "555-123-4567",
                Address = "123 Test St",
                Segment = segment,
                SignupDate = DateTime.UtcNow.AddDays(-60)
            };

            try
            {
                // Act - Remove the leading slash
                var response = await _client.PostAsJsonAsync("api/customers", createCustomerRequest);

                // Debug information if needed
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"HTTP Error: {response.StatusCode}, Content: {errorContent}");
                    // You could also log this to the test output
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

    // Request models remain the same
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CustomerSegment Segment { get; set; }
        public DateTime SignupDate { get; set; }
    }

    // Other request models remain the same...
}
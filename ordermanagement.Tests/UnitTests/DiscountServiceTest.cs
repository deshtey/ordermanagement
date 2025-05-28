// OrderManagement.Tests/UnitTests/DiscountServiceTests.cs
using Moq;
using ordermanagement.application.Contracts;
using ordermanagement.application.Contracts.InMemoryRepo;
using ordermanagement.application.Services;
using ordermanagement.domain.Entities;
using ordermanagement.infrastructure.InMemoryRepo;

namespace ordermanagement.Tests.UnitTests;
public class DiscountServiceTests
{
    [Fact]
    public async Task CalculateDiscount_VipCustomer_AppliesCorrectDiscount()
    {
        // Arrange
        // Setup mocks
        var mockDiscountRepo = new Mock<InMemoryDiscountRepository>();
        var mockOrderRepo = new Mock<InMemoryOrderRepo>();
        var mockCustomerRepo = new Mock<InMemoryCustomerRepo>();
        int CustomerId = 2;

        // Create service instances
        var discountService = new DiscountService(
            mockDiscountRepo.Object,
            mockOrderRepo.Object);

        var orderService = new OrderService(
            mockOrderRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        // Create test data
        var customer = new Customer(
            "Test Customer",
            "z2N4M@example.com",
            "1234567890",
            "123 Test St, Test City, TC 12345",
            CustomerSegment.VIP)
        {
            Id = CustomerId
        };


        var orderItem = new OrderItem(100, 100, 12, 12);
        var order = new Order(
            customer.Id,
            customer.Address,
            "456 Test St, Test City, TC 12345",
            new List<OrderItem> { orderItem });
        order.Customer = customer;

        decimal Expected = 0.015m * orderItem.Quantity * orderItem.UnitPrice;

        // Act
        await orderService.CreateOrderAsync(order);
        var discount = await discountService.CalculateDiscountAmountAsync(order);

        // Assert
        Assert.Equal(Expected, discount);
    }

    [Fact]
    public async Task CalculateDiscount_FifthOrder_AppliesLoyaltyDiscount()
    {
        // Arrange
        var customerId = 1;
        var previousOrders = Enumerable.Range(1, 4).Select(_ => new Order(
            2,
            "456 Oak Ave",
            "456 Oak Ave",
            new List<OrderItem>
            {
                    new OrderItem(2, 1, 1, 0.10m),
                    new OrderItem(2, 1, 1, 0.10m),
                    new OrderItem(2, 1, 1, 0.10m),
                    new OrderItem(2, 1, 1, 0.10m)
            }
        )).ToList();

        var mockDiscountRepo = new Mock<InMemoryDiscountRepository>();
        var mockOrderRepo = new Mock<InMemoryOrderRepo>();
        var mockCustomerRepo = new Mock<InMemoryCustomerRepo>();


        var strategies = new List<IDiscountStrategy>
        {
            new OrderHistoryDiscountStrategy(mockOrderRepo.Object)
        };

        var discountService = new DiscountService(
            mockDiscountRepo.Object,
            mockOrderRepo.Object);
        var orderService = new OrderService(
            mockOrderRepo.Object,
            mockCustomerRepo.Object,
            discountService);
        var customer = new Customer("Test Customer", "z2N4M@example.com", "1234567890", "123 Test St, Test City, TC 12345", CustomerSegment.Regular);
        customer.Id = customerId;

        var order = new Order(customer.Id, customer.Address, "456 Test St, Test City, TC 12345", new List<OrderItem> {
        new OrderItem(100, 100, 12, 12),});

        // Act
        decimal Expected = 0.07m * 12 * 12;

        // Act
        await orderService.CreateOrderAsync(order);
        var discount = await discountService.CalculateDiscountAmountAsync(order);

        // Assert
        Assert.Equal(Expected, discount);
    }

    [Fact]
    public async Task CalculateDiscountAmount_FirstOrderWithPromoCode_ReturnsCorrectDiscount()
    {
        // Arrange
        string promoCode = "FIRSTORDER10";
        decimal expectedDiscount = 134.4m; 

        var mockDiscountRepo = new Mock<IDiscountRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();

        // Setup discount repository mock
        mockDiscountRepo.Setup(d => d.GetByPromoCodeAsync(promoCode))
            .ReturnsAsync(new Discount(
                "First Order Discount",
                promoCode,
                0.10m,
                0,
                new List<CustomerSegment> { CustomerSegment.Regular },
                null)
            { Id = 1 });

        // Create test data
        var customer = new Customer("Jane Ngoiri", "jane@gmail.com", "0723985743", "435 Kiambu", CustomerSegment.Regular);
        var order = new Order(customer.Id, customer.Address, customer.Address,
            [
                new OrderItem(101, 102, 12, 12m),
            new OrderItem(101, 103, 100, 12m)
            ])
        {
            Id = 101,
            Customer = customer
        };

        var discountService = new DiscountService(mockDiscountRepo.Object, mockOrderRepo.Object);

        // Act
        var actualDiscount = await discountService.CalculateDiscountAmountAsync(order, promoCode);

        // Assert
        Assert.Equal(expectedDiscount, actualDiscount);
        mockDiscountRepo.Verify(d => d.GetByPromoCodeAsync(promoCode), Times.Once);
    }
}
using ordermanagement.domain.Entities;
using ordermanagement.domain.Enums;

namespace ordermanagement.infrastructure.TestData
{
    public static class SeedData
    {
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer("Simon Ndiritu", "deshtey@gmail.com", "111-222-3333", "123 Nairobi", CustomerSegment.Regular)
                {
                    Id = 1
                },
                new Customer("Robert Kamungu", "bob@example.com", "444-555-6666", "456 Ruiru", CustomerSegment.VIP)
                {
                    Id = 2
                },
                new Customer("Charlie Wanjiru", "charlie@example.com", "087-654-3210", "789 Pipeline", CustomerSegment.NewCustomer)
                {
                    Id = 3
                },
                new Customer("John Doe", "john@example", "123-456-7890", "74 Mombasa", CustomerSegment.Regular)
                {
                    Id = 4
                },
                new Customer("Peninah Wangai", "peninah@example.com", "0723-456-7890", "56 Nakuru", CustomerSegment.Premium)
                {
                    Id = 5
                },

            };
        }

        public static List<Discount> GetDiscounts()
        {
            return new List<Discount> {
                new Discount("Loyalty Discount", "Loyal50", 0.07m,4, new List<CustomerSegment> { CustomerSegment.Regular, CustomerSegment.Premium }, null)
                { Id = 1 },
                new Discount("New Customer Discount", "NEWCUST10", 0.05m,0,new List<CustomerSegment> { CustomerSegment.NewCustomer}, null)
                { Id = 2 },
                new Discount("VIP Discount", "VIP15", 0.015m,0, new List<CustomerSegment> { CustomerSegment.VIP }, null)
                { Id = 3 },
                new Discount("Premium Discount", "Premium10", 0.1m, 0, new List<CustomerSegment> { CustomerSegment.Premium },null)
                { Id =  4},


            };
        }

        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product("Laptop Pro", 1200.00m, 111, 10)
                {
                    Id = 1
                },
                new Product("Wireless Mouse", 25.00m, 222, 50)
                {
                    Id = 2
                },
                new Product("Mechanical Keyboard", 80.00m, 333, 20)
                {
                    Id = 3
                },
                new Product("Headphones", 50.00m, 444, 30)
                {
                    Id = 4
                },
                new Product("USB Cable", 10.00m, 555, 100)
                {
                    Id = 5
                }

            };
        }

        public static List<Order> GetOrders()
        {
            var customers = GetCustomers();
            var products = GetProducts();

            var order1 = new Order(
                customers[0].Id,
                customers[0].Address,
                customers[0].Address,
                [
                    new OrderItem(1, products[0].Id, 12, products[0].UnitPrice),
                    new OrderItem(1, products[1].Id, 100, products[1].UnitPrice)
                ]
            )
            {
                Id = 1,
                OrderDate = DateTime.UtcNow.AddDays(-8),
                ShippedDate = DateTime.UtcNow.AddDays(-7),
                Status = OrderStatus.Delivered

            };


            var order2 = new Order(
                customers[1].Id,
                "456 Oak Ave",
                "456 Oak Ave",
                new List<OrderItem>
                {
                    new OrderItem(2, products[2].Id, 1, products[2].UnitPrice),
                    new OrderItem(2, products[1].Id, 10, products[1].UnitPrice),
                    new OrderItem(2, products[0].Id, 5, products[0].UnitPrice),
                    new OrderItem(2, products[3].Id, 13, products[3].UnitPrice)

                }
            )
            {
                Id = 2,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                Status = OrderStatus.Shipped,            
            };
            order2.ApplyDiscounts(new List<Discount>  { GetDiscounts()[2] });
            var order3 = new Order(
                customers[3].Id,
                            customers[3].Address,
                            customers[3].Address,
                new List<OrderItem>
                {
                                new OrderItem(3, products[3].Id, 1, products[3].UnitPrice),
                                new OrderItem(3, products[1].Id, 1, products[1].UnitPrice),
                                new OrderItem(3, products[0].Id, 3, products[0].UnitPrice),
                                new OrderItem(3, products[4].Id, 7, products[4].UnitPrice)

                }
)
            {
                Id = 3,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                Status = OrderStatus.Processing
            };
            var order4 = new Order(
                 customers[0].Id,
                 customers[0].Address,
                 customers[0].Address,
                 [
                     new OrderItem(products[3].Id, products[3].Id, 6, products[3].UnitPrice),
                                new OrderItem(4,  products[1].Id, 100, products[1].UnitPrice)
                 ]
             )
            {
                Id = 4,
                OrderDate = DateTime.UtcNow.AddDays(-1),
            };

            return new List<Order> { order1, order2, order3, order4 };
        }
    }
}
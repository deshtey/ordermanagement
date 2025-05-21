using Microsoft.Extensions.DependencyInjection;
using ordermanagement.application.Contracts;
using ordermanagement.application.Contracts.InMemoryRepo;
using ordermanagement.infrastructure.InMemoryRepo;
using ordermanagement.infrastructure.TestData;

namespace ordermanagement.infrastructure
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //var customers = SeedData.GetCustomers();
            //var products = SeedData.GetProducts();
            //var orders = SeedData.GetOrders();
            services.AddSingleton<IProductRepository, InMemoryProductRepo>();
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepo>();
            services.AddSingleton<IOrderRepository, InMemoryOrderRepo>();
            services.AddSingleton<IDiscountRepository, InMemoryDiscountRepository>();
            return services;
        }
    }
}

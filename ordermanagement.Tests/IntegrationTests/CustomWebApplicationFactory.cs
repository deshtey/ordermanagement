using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ordermanagement.application.Contracts;
using ordermanagement.application.Contracts.InMemoryRepo;
using ordermanagement.application.Services;
using ordermanagement.infrastructure.InMemoryRepo;
using ordermanagement.infrastructure.TestData;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //var projectDir = Directory.GetCurrentDirectory();
        //var projectParent = Directory.GetParent(projectDir).FullName;
        //var mainAppPath = Path.Combine(projectParent, "ordermanagement");

        //builder.UseContentRoot(mainAppPath);

        builder.ConfigureServices(services =>
        {
            var customers = SeedData.GetCustomers();
            var products = SeedData.GetProducts();
            var orders = SeedData.GetOrders();


            services.AddSingleton<IProductRepository, InMemoryProductRepo>();
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepo>();
            services.AddSingleton<IOrderRepository, InMemoryOrderRepo>();
            services.AddSingleton<IDiscountRepository, InMemoryDiscountRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddSingleton<IDiscountService, DiscountService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IDiscountStrategy, DiscountStrategy>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();
            services.AddScoped<IOrderAnalyticsService, OrderAnalyticsService>();
            //services.AddInfrastructure();
            //services.AddApplication();
            //services.AddHttpClient();
        });

        builder.UseEnvironment("Development");
    }
}
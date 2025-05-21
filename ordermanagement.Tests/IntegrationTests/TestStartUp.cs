using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ordermanagement.application.Contracts;
using ordermanagement.application.Contracts.InMemoryRepo;
using ordermanagement.application.Services;
using ordermanagement.infrastructure.InMemoryRepo;
using System;
using System.IO;

namespace OrderManagement.Tests.IntegrationTests
{
    /// <summary>
    /// Test startup configuration for integration tests
    /// This mimics the real Startup.cs but uses in-memory databases and test-specific services
    /// </summary>
    public class TestStartup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IWebHostEnvironment env)
        {
            // Set up configuration from appsettings.json and appsettings.Test.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Test.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add controllers and API functionality
            services.AddControllers()
                .AddApplicationPart(typeof(TestStartup).Assembly); // This includes controllers from the API project

            //// Register in-memory database for testing
            //services.AddDbContext<OrderManagementDbContext>(options =>
            //    options.UseInMemoryDatabase(databaseName: $"OrderManagementTest-{Guid.NewGuid()}"));

            // Register repositories
            services.AddScoped<ICustomerRepository, InMemoryCustomerRepo>();
            services.AddScoped<IProductRepository, InMemoryProductRepo>();
            services.AddScoped<IOrderRepository, InMemoryOrderRepo>();
            services.AddScoped<IDiscountRepository, InMemoryDiscountRepository>();

            // Register services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDiscountService, DiscountService>();


            // Add any other test-specific services here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // Configure error handling for tests
            app.UseExceptionHandler("/error");

            // Enable routing
            app.UseRouting();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }

}
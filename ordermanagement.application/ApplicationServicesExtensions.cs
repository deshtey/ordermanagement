using Microsoft.Extensions.DependencyInjection;
using ordermanagement.application.Contracts;
using ordermanagement.application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ordermanagement.application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddSingleton<IDiscountService, DiscountService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IDiscountStrategy, DiscountStrategy>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();
            services.AddScoped<IOrderAnalyticsService, OptimizedOrderAnalyticsService>();
            services.AddMemoryCache();
            return services;
        }
    }
}

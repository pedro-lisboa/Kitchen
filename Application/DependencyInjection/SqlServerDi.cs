using Application.Contexts;
using Domain.AggregatesModel.OrderAggregate.Interface;
using Domain.AggregatesModel.OrderProductAggregate.Interface;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Infrastructure.Queries;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public class SqlServerDi
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            // Orders
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<OrderQuery>();
            services.AddSingleton<IOrderQuery, OrderQuery>();
            // Products
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ProductQuery>();
            services.AddSingleton<IProductQuery, ProductQuery>();
            // OrderProducts
            services.AddSingleton<IOrderProductRepository, OrderProductRepository>();
            services.AddSingleton<OrderProductQuery>();
            services.AddSingleton<IOrderProductQuery, OrderProductQuery>();

            services.AddSingleton<OrderContext>(x =>
            {
                OrderContext orderContext = new OrderContext(x.GetRequiredService<OrderQuery>());
                return orderContext;
            });
            services.AddSingleton<ProductContext>(x =>
            {
                ProductContext productContext = new ProductContext(x.GetRequiredService<ProductQuery>());
                return productContext;
            });
            services.AddSingleton<ProductOrderContext>(x =>
            {
                ProductOrderContext orderProductContext = new ProductOrderContext(x.GetRequiredService<OrderProductQuery>());
                return orderProductContext;
            });
        }
    }
}
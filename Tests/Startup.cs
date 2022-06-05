using Application.Contexts;
using Domain.AggregatesModel.OrderAggregate.Interface;
using Infrastructure.Queries;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration.GetConfiguration());

            var mockConfiguration = new Mock<IConfiguration>();
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            services.AddSingleton<IConfiguration>(Configuration.GetConfiguration());
            services.AddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);

            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderQuery, OrderQuery>();

            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderQuery, OrderQuery>();

            services.AddScoped<OrderContext>();
        }
    }
}
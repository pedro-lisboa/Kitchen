using Application.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public class ContextsDi
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Context scoped for faster access to data in the instance.
            // The fast fail validation and operation execution pipeline will look for id at various times
            services.AddScoped<OrderContext>();
            services.AddScoped<ProductContext>();
        }
    }
}
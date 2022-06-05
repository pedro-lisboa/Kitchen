using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.DependencyInjection
{
    public class MediatorDi
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddMediatR(typeof(MediatorDi).GetTypeInfo().Assembly);

            services.AddValidatorsFromAssembly(typeof(MediatorDi).GetTypeInfo().Assembly);
        }
    }
}
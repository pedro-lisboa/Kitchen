using Application.DependencyInjection;
using Domain.AggregatesModel;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("KitchenCorsPolicy", builder =>
                {
                    builder
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      //.SetIsOriginAllowed(MyIsOriginAllowed) 
                      .WithOrigins(_configuration.GetSection("Url:Kitchen").Value);
                });
            });

            services.AddProblemDetails(ConfigureProblemDetails)
                .AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation();

            services.AddMemoryCache();

            ContextsDi.ConfigureServices(_configuration, services);
            SqlServerDi.ConfigureServices(_configuration, services);
            MediatorDi.ConfigureServices(_configuration, services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("management-order", new OpenApiInfo { Title = "Management - Order", Version = "v1" });
                c.SwaggerDoc("management-product", new OpenApiInfo { Title = "Management - Product", Version = "v1" });

                c.SwaggerDoc("orders", new OpenApiInfo { Title = "Orders", Version = "v1" });
                c.SwaggerDoc("products", new OpenApiInfo { Title = "Products", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer abcd1234\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                //c.AddFluentValidationRules();
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            app.Use(CustomMiddleware);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors("KitchenCorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("swagger/management-order/swagger.json", "Management Order");
                opt.SwaggerEndpoint("swagger/management-product/swagger.json", "Management Product");

                opt.DocumentTitle = "Kitchen API Documentation";
                opt.DocExpansion(DocExpansion.None);
                opt.RoutePrefix = string.Empty;
                opt.EnableDeepLinking();
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // Only include exception details in a development environment. There's really no nee
            // to set this as it's the default behavior. It's just included here for completeness :)
            //options.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();

            // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
            // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
            options.Rethrow<NotSupportedException>();

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }

        private Task CustomMiddleware(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.StartsWithSegments("/middleware", out _, out var remaining))
            {
                if (remaining.StartsWithSegments("/error"))
                {
                    throw new Exception("This is an exception thrown from middleware.");
                }

                if (remaining.StartsWithSegments("/status", out _, out remaining))
                {
                    var statusCodeString = remaining.Value.Trim('/');

                    if (int.TryParse(statusCodeString, out var statusCode))
                    {
                        context.Response.StatusCode = statusCode;
                        return Task.CompletedTask;
                    }
                }
            }

            return next();
        }

        private static bool MyIsOriginAllowed(string origin)
        {
            var isAllowed = false;

            // Your logic.

            return isAllowed;
        }
    }
}

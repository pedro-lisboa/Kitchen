using Application.Commands.Orders;
using Application.Contexts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Filters
{
    public class ExistsOrderFilter : IAsyncActionFilter
    {
        private readonly OrderContext _orderContext;

        public ExistsOrderFilter(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IDictionary<string, object> _order = context.ActionArguments;
            var command = _order["command"];
            var id = command.GetType().GetProperty("Id").GetValue(command) as string;

            if (id == null)
            {
                var commandObj = (UpdateOrderCommand)command;
                id = commandObj.Id.ToString();
            }

            var result = await _orderContext.GetById(id);

            if (result == null)
            {
                throw new ProblemDetailsException(404, "Route id is not exists.");
            }
            else
            {
                await next();
            }
        }
    }
}
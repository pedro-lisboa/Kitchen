using Application.Commands.Orders;
using Application.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Api.Filters
{
    public class PutConsistenceFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IDictionary<string, object> cxt = context.ActionArguments;
            var command = cxt["command"];
            var routeId = command.GetType().GetProperty("Id").GetValue(command) as string;

            if (routeId == null)
            {
                var commandObj = (UpdateOrderCommand)command;
                routeId = commandObj.Id.ToString();
            }

            if (!context.ActionArguments.ContainsKey("command"))
            {
                throw new ProblemDetailsException(500, "Internal parameterization error.");
            }

            var model = context.ActionArguments["command"] as IPutCommand;

            if (model == null)
            {
                throw new ProblemDetailsException(400, "Invalid request body for http verb.");
            }

            if (model.Id.ToString() != routeId)
            {
                throw new ProblemDetailsException(400, "Route id and command id are different.");
            }
        }
    }
}
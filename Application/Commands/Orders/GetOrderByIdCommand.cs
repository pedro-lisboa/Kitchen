using Domain.AggregatesModel.OrderAggregate;
using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.Orders
{
    public class GetOrderByIdCommand : IRequest<Order>
    {
        public int Id { get; set; }
    }
}
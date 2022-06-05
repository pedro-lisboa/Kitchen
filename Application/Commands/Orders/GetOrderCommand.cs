using Domain.AggregatesModel.OrderAggregate;
using MediatR;
using System.Collections.Generic;

namespace Application.Commands.Orders
{
    public class GetOrderCommand : IRequest<IEnumerable<Order>>
    {
        public int Size { get; set; }

        public int Offset { get; set; }
    }
}
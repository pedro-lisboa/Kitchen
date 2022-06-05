using Domain.AggregatesModel.ProductAggregate;
using Domain.Dtos.Order;
using MediatR;
using System.Collections.Generic;

namespace Application.Commands.Orders
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public int TableNum { get; set; }

        public IEnumerable<ProductInOrder> items { get; set; }
    }
}
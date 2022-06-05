using Application.Interfaces;
using Domain.AggregatesModel.ProductAggregate;
using Domain.Dtos.Order;
using MediatR;
using System.Collections.Generic;

namespace Application.Commands.Orders
{
    public class UpdateOrderCommand : IRequest<OrderDto>, IPutCommand
    {
        public int Id { get; set; }

        public int TableNum { get; set; }

        public IEnumerable<ProductInOrder> items { get; set; }
    }
}
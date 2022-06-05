using Domain.Dtos.Order;
using MediatR;

namespace Application.Commands.Orders
{
    public class CallOrderCommand : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }
}
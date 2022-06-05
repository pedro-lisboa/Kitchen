using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.Orders
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
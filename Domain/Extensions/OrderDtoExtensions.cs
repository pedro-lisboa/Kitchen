using Domain.AggregatesModel.OrderAggregate;
using Domain.Dtos.Order;

namespace Domain.Extensions
{
    public static class OrderExtensions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto()
            {
                Queue = order.Id,
            };
        }
    }
}
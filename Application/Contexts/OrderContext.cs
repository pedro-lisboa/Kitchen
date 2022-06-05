using Domain.AggregatesModel.OrderAggregate;
using Domain.AggregatesModel.OrderAggregate.Interface;
using Domain.Dtos.Order;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contexts
{
    public class OrderContext : IOrderQuery
    {
        private readonly IOrderQuery _queries;

        private ConcurrentDictionary<string, OrderDto> _getByIds;

        public OrderContext(IOrderQuery queries)
        {
            _queries = queries;
            _getByIds = new ConcurrentDictionary<string, OrderDto>();
        }

        public Task<IEnumerable<Order>> GetAllAsync(int size, int offset)
        {
            return _queries.GetAllAsync(size, offset);
        }

        public async Task<Order> GetById(string id)
        {
            var order = await _queries.GetById(id);

            return order;
        }
    }
}
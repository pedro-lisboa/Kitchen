using Domain.AggregatesModel.OrderAggregate;
using Domain.AggregatesModel.OrderProductAggregate.Interface;
using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Domain.Dtos.Product;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contexts
{
    public class ProductOrderContext : IOrderProductQuery
    {
        private readonly IOrderProductQuery _queries;

        private ConcurrentDictionary<string, ProductDto> _getByIds;

        public ProductOrderContext(IOrderProductQuery queries)
        {
            _queries = queries;
            _getByIds = new ConcurrentDictionary<string, ProductDto>();
        }

        public async Task<IEnumerable<OrderProduct>> GetAllAsync(string id)
        {
            var orderProduct = await _queries.GetAllAsync(id);

            return orderProduct;
        }
    }
}
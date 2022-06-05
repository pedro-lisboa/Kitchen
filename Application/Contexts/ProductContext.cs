using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Domain.Dtos.Product;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contexts
{
    public class ProductContext : IProductQuery
    {
        private readonly IProductQuery _queries;

        private ConcurrentDictionary<string, ProductDto> _getByIds;

        public ProductContext(IProductQuery queries)
        {
            _queries = queries;
            _getByIds = new ConcurrentDictionary<string, ProductDto>();
        }

        public Task<IEnumerable<Product>> GetAllAsync(int size, int offset)
        {
            return _queries.GetAllAsync(size, offset);
        }

        public async Task<Product> GetById(int? id)
        {
            var product = await _queries.GetById(id);

            return product;
        }
    }
}
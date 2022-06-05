using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ProductAggregate.Interface
{
    public interface IProductQuery
    {
        Task<IEnumerable<Product>> GetAllAsync(int size, int offset);

        Task<Product> GetById(int? id);
    }
}
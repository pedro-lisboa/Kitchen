using Domain.AggregatesModel.ProductAggregate;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ProductAggregate.Interface
{
    public interface IProductRepository
    {
        Task<int> InsertAsync(Product product);
    }
}
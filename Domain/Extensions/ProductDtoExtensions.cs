using Domain.AggregatesModel.ProductAggregate;
using Domain.Dtos.Product;

namespace Domain.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
            };
        }
    }
}
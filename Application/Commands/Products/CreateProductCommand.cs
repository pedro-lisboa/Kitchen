using Domain.Dtos.Product;
using MediatR;

namespace Application.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public decimal Price { get; set; }
    }
}
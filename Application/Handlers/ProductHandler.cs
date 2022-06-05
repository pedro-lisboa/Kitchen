using Application.Commands.Products;
using Application.Contexts;
using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Domain.Dtos.Product;
using Domain.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ProductHandler : IRequestHandler<CreateProductCommand, ProductDto>,
                                    IRequestHandler<GetProductCommand, IEnumerable<Product>>
    {

        private readonly IProductRepository _repository;

        private readonly ProductContext _context;

        public ProductHandler(IProductRepository repository, ProductContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product inserted = new Product();
            inserted.Name = request.Name;
            inserted.Area = request.Area;
            inserted.Price = request.Price;

            inserted.Id = await _repository.InsertAsync(inserted);

            return inserted.ToDto();
        }

        public async Task<IEnumerable<Product>> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _context.GetAllAsync(request.Size, request.Offset);

            if (products.Count() != 0)
            {
                return products;
            }

            return default;
        }
    }
}
using Domain.AggregatesModel.ProductAggregate;
using MediatR;
using System.Collections.Generic;

namespace Application.Commands.Products
{
    public class GetProductCommand : IRequest<IEnumerable<Product>>
    {
        public int Size { get; set; }

        public int Offset { get; set; }
    }
}
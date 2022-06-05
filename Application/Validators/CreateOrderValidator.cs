using Application.Commands.Orders;
using Application.Commands.Products;
using Application.Contexts;
using Domain.AggregatesModel.ProductAggregate;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Application.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        private bool ItemsLength(IEnumerable<ProductInOrder> items)
        {
            if (items == null)
            {
                return false;
            }
            return true;
        }

        public CreateOrderValidator()
        {
            RuleFor(x => x.TableNum)
                .GreaterThan(0)
                .LessThanOrEqualTo(int.MaxValue);

            RuleFor(x => x.items)
                .NotNull()
                .NotEmpty()
                .Must(ItemsLength).WithMessage("'Items' must have at least 1 item");
        }
    }
}
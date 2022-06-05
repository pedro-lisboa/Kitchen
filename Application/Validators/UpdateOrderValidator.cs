using Application.Commands.Orders;
using Application.Contexts;
using Domain.AggregatesModel.ProductAggregate;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Application.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {

        private bool ItemsLength(IEnumerable<ProductInOrder> items)
        {
            if (items == null)
            {
                return false;
            }
            return true;
        }

        public UpdateOrderValidator()
        {
            RuleFor(x => x.TableNum)
                .GreaterThan(0)
                .LessThan(int.MaxValue);

            RuleFor(x => x.items)
                .NotNull()
                .NotEmpty()
                .Must(ItemsLength).WithMessage("'Items' must contain at least one product");
        }
    }
}
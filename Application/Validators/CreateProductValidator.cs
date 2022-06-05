using Application.Commands.Products;
using Domain.AggregatesModel.ProductAggregate;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        private bool AreaValidation(string value)
        {
            if (value == null)
            {
                return false;
            }

            string area = value as string;
            string[] areas = { "fries", "grill", "salad", "drink", "desert" };

            if (!areas.Contains(area.ToLower()))
            {
                return false;
            }

            return true;
        }

        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThanOrEqualTo(decimal.MaxValue);

            RuleFor(x => x.Area)
                .NotNull()
                .NotEmpty()
                .Must(AreaValidation).WithMessage("'Area' must be fries, grill, salad, drink or desert");
        }
    }
}
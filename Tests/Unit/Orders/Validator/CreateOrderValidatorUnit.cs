using Application.Commands.Orders;
using Application.Contexts;
using Application.Handlers;
using Application.Validators;
using Domain.AggregatesModel.OrderAggregate;
using Domain.AggregatesModel.OrderAggregate.Interface;
using Domain.AggregatesModel.OrderProductAggregate.Interface;
using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Orders.Handlers
{
    [Collection("Order Collection")]
    public class CreateOrderValidatorUnit
    {
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(int.MaxValue, true)]
        [InlineData(int.MinValue, false)]
        [Trait("Category", "Unit")]
        public void TableNumValidate(int TableNum, bool isValid)
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TableNum = TableNum,
                items = new List<ProductInOrder>
                {
                    new ProductInOrder
                    {
                        Id = 1,
                        Amount = 2,
                        Note = "no picles"
                    }
                }
            };

            CreateOrderValidator validator = new CreateOrderValidator();

            var validatorResult = validator.Validate(command);

            Assert.Equal(isValid, validatorResult.IsValid);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void OneItemValidate()
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TableNum = 1,
                items = new List<ProductInOrder>
                {
                    new ProductInOrder
                    {
                        Id = 1,
                        Amount = 2,
                        Note = "no picles"
                    }
                }
            };

            CreateOrderValidator validator = new CreateOrderValidator();

            var validatorResult = validator.Validate(command);

            Assert.True(validatorResult.IsValid);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TwoOrMoreItemsValidate()
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TableNum = 1,
                items = new List<ProductInOrder>
                {
                    new ProductInOrder
                    {
                        Id = 1,
                        Amount = 2,
                        Note = "no picles"
                    },
                    new ProductInOrder
                    {
                        Id = 2,
                        Amount = 1,
                        Note = ""
                    }
                }
            };

            CreateOrderValidator validator = new CreateOrderValidator();

            var validatorResult = validator.Validate(command);

            Assert.True(validatorResult.IsValid);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void NullItemValidate()
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TableNum = 1,
                items = null
            };

            CreateOrderValidator validator = new CreateOrderValidator();

            var validatorResult = validator.Validate(command);

            Assert.False(validatorResult.IsValid);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void NoItemValidate()
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TableNum = 1,
                items = new List<ProductInOrder>()
            };

            CreateOrderValidator validator = new CreateOrderValidator();

            var validatorResult = validator.Validate(command);

            Assert.False(validatorResult.IsValid);
        }
    }
}
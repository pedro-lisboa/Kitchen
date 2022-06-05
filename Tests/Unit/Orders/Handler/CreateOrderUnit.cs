using Application.Commands.Orders;
using Application.Contexts;
using Application.Handlers;
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
    public class CreateOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
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

            Product product = new Product()
            {
                Id = 1,
                Name = "Hamburguer",
                Area = "grill",
                Price = 9.00m
            };

            OrderProduct orderProduct = new OrderProduct()
            {
                Amount = 2,
                Note = "no picles",
                OrderId = 1,
                ProductId = 1
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.InsertAsync(It.IsAny<Order>())).Returns(Task.FromResult(1));
            mockProductQuery.Setup(_ => _.GetById(It.IsAny<int>())).Returns(Task.FromResult(product));
            mockOrderProductRepository.Setup(_ => _.InsertAsync(It.IsAny<OrderProduct>())).Returns(Task.FromResult(1));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderRepository.Verify(_ => _.InsertAsync(It.IsAny<Order>()), Times.Once());
            mockProductQuery.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.InsertAsync(It.IsAny<OrderProduct>()), Times.Once());

            result.Should().NotBeNull();
            Assert.Equal(1, result.Queue);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task HasNoProductRegistered()
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

            Product product = null;

            OrderProduct orderProduct = new OrderProduct()
            {
                Amount = 2,
                Note = "no picles",
                OrderId = 1,
                ProductId = 1
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.InsertAsync(It.IsAny<Order>())).Returns(Task.FromResult(1));
            mockProductQuery.Setup(_ => _.GetById(It.IsAny<int>())).Returns(Task.FromResult(product));
            mockOrderProductRepository.Setup(_ => _.InsertAsync(It.IsAny<OrderProduct>())).Returns(Task.FromResult(1));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => orderHandler.Handle(command, CancellationToken.None));

            mockOrderRepository.Verify(_ => _.InsertAsync(It.IsAny<Order>()), Times.Once());
            mockProductQuery.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.InsertAsync(It.IsAny<OrderProduct>()), Times.Never());

            Assert.Equal("All items must be previously registered", ex.Message);
        }
    }
}
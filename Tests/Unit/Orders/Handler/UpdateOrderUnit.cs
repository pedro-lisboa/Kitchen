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
    public class UpdateOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
        {
            UpdateOrderCommand command = new UpdateOrderCommand()
            {
                Id = 1,
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

            Order order = new Order()
            {
                Id = 1,
                Active = 1,
                TableNum = 1,
                items = new List<ProductInOrder>(),
                CreatedAt = DateTime.Now
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockProductQuery.Setup(_ => _.GetById(It.IsAny<int>())).Returns(Task.FromResult(product));
            mockOrderProductRepository.Setup(_ => _.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            mockOrderProductRepository.Setup(_ => _.InsertAsync(It.IsAny<OrderProduct>())).Returns(Task.FromResult(1));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderRepository.Verify(_ => _.UpdateAsync(It.IsAny<Order>()), Times.Once());
            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockProductQuery.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.DeleteAsync(It.IsAny<string>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.InsertAsync(It.IsAny<OrderProduct>()), Times.Once());

            result.Should().NotBeNull();
            Assert.Equal(1, result.Queue);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task OrderNotFoundRegistered()
        {
            UpdateOrderCommand command = new UpdateOrderCommand()
            {
                Id = 1,
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
            Order order = null;

            Product product = null;

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockProductQuery.Setup(_ => _.GetById(It.IsAny<int>())).Returns(Task.FromResult(product));
            mockOrderProductRepository.Setup(_ => _.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            mockOrderProductRepository.Setup(_ => _.InsertAsync(It.IsAny<OrderProduct>())).Returns(Task.FromResult(1));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockProductQuery.Verify(_ => _.GetById(It.IsAny<int>()), Times.Never());
            mockOrderRepository.Verify(_ => _.UpdateAsync(It.IsAny<Order>()), Times.Never());
            mockOrderProductRepository.Verify(_ => _.DeleteAsync(It.IsAny<string>()), Times.Never());
            mockOrderProductRepository.Verify(_ => _.InsertAsync(It.IsAny<OrderProduct>()), Times.Never());

            result.Should().BeNull();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task HasNoProductRegistered()
        {
            UpdateOrderCommand command = new UpdateOrderCommand()
            {
                Id = 1,
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
            Order order = new Order()
            {
                Active = 1,
                Id = 1,
                TableNum = 1,
                CreatedAt = DateTime.Now,
                items = new List<ProductInOrder>()
            };

            Product product = null;

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockProductQuery.Setup(_ => _.GetById(It.IsAny<int>())).Returns(Task.FromResult(product));
            mockOrderProductRepository.Setup(_ => _.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            mockOrderProductRepository.Setup(_ => _.InsertAsync(It.IsAny<OrderProduct>())).Returns(Task.FromResult(1));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => orderHandler.Handle(command, CancellationToken.None));

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockProductQuery.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once());
            mockOrderRepository.Verify(_ => _.UpdateAsync(It.IsAny<Order>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.DeleteAsync(It.IsAny<string>()), Times.Once());
            mockOrderProductRepository.Verify(_ => _.InsertAsync(It.IsAny<OrderProduct>()), Times.Never());

            Assert.Equal("All items must be previously registered", ex.Message);
        }
    }
}
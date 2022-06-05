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
    public class CallOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
        {
            CallOrderCommand command = new CallOrderCommand()
            {
                Id = 1,
            };

            Order order = new Order()
            {
                Id = 1,
                Active = 1
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockOrderRepository.Setup(_ => _.CallUpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockOrderRepository.Verify(_ => _.CallUpdateAsync(It.IsAny<Order>()), Times.Once());

            result.Should().NotBeNull();
            Assert.Equal(1, result.Queue);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task HasNoProductRegistered()
        {
            CallOrderCommand command = new CallOrderCommand()
            {
                Id = 1,
            };

            Order order = null;

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockOrderRepository.Setup(_ => _.CallUpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockOrderRepository.Verify(_ => _.CallUpdateAsync(It.IsAny<Order>()), Times.Never());

            result.Should().BeNull();
        }
    }
}
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
    public class DeleteOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
        {
            DeleteOrderCommand command = new DeleteOrderCommand()
            {
                Id = "1",
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderRepository.Verify(_ => _.DeleteAsync(It.IsAny<string>()), Times.Once());

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EmptyId()
        {
            DeleteOrderCommand command = new DeleteOrderCommand()
            {
                Id = null,
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderRepository.Setup(_ => _.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderRepository.Verify(_ => _.DeleteAsync(It.IsAny<string>()), Times.Never());

            Assert.False(result);
        }
    }
}
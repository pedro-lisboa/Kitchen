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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Orders.Handlers
{
    [Collection("Order Collection")]
    public class GetOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
        {
            GetOrderCommand command = new GetOrderCommand()
            {
                Offset = 10,
                Size = 1
            };

            ProductInOrder productInOrder = new ProductInOrder()
            {
                Id = 1,
                Amount = 2,
                Note = ""
            };

            Order order1 = new Order()
            {
                Id = 1,
                Active = 1,
                items = new List<ProductInOrder>()
                {
                    productInOrder
                }
            };

            Order order2 = new Order()
            {
                Id = 1,
                Active = 1,
                items = new List<ProductInOrder>()
                {
                    productInOrder,
                    productInOrder
                }
            };

            IEnumerable<Order> listOrder = new List<Order>()
            {
                order1,
                order2
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderQuery.Setup(_ => _.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(listOrder));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());

            result.Should().NotBeNull();
            Assert.Equal(listOrder.ToList().Count, result.Count());
            Assert.Equal(listOrder.ToList()[0].Active, result.ToList()[0].Active);
            Assert.Equal(listOrder.ToList()[0].CreatedAt, result.ToList()[0].CreatedAt);
            Assert.Equal(listOrder.ToList()[0].Id, result.ToList()[0].Id);
            Assert.Equal(listOrder.ToList()[0].items.ToList()[0].Id, result.ToList()[0].items.ToList()[0].Id);
            Assert.Equal(listOrder.ToList()[0].items.ToList()[0].Note, result.ToList()[0].items.ToList()[0].Note);
            Assert.Equal(listOrder.ToList()[0].items.ToList()[0].Amount, result.ToList()[0].items.ToList()[0].Amount);
            Assert.Equal(listOrder.ToList()[1].Active, result.ToList()[1].Active);
            Assert.Equal(listOrder.ToList()[1].CreatedAt, result.ToList()[1].CreatedAt);
            Assert.Equal(listOrder.ToList()[1].Id, result.ToList()[1].Id);
            Assert.Equal(listOrder.ToList()[1].items.ToList()[0].Id, result.ToList()[1].items.ToList()[0].Id);
            Assert.Equal(listOrder.ToList()[1].items.ToList()[0].Note, result.ToList()[1].items.ToList()[0].Note);
            Assert.Equal(listOrder.ToList()[1].items.ToList()[0].Amount, result.ToList()[1].items.ToList()[0].Amount);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task NullResponse()
        {
            GetOrderCommand command = new GetOrderCommand()
            {
                Offset = 10,
                Size = 1
            };

            ProductInOrder productInOrder = new ProductInOrder()
            {
                Id = 1,
                Amount = 2,
                Note = ""
            };

            Order order1 = new Order()
            {
                Id = 1,
                Active = 1,
                items = new List<ProductInOrder>()
                {
                    productInOrder
                }
            };

            Order order2 = new Order()
            {
                Id = 1,
                Active = 1,
                items = new List<ProductInOrder>()
                {
                    productInOrder,
                    productInOrder
                }
            };

            IEnumerable<Order> listOrder = new List<Order>();

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderQuery.Setup(_ => _.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(listOrder));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());

            result.Should().BeNullOrEmpty();
            Assert.Null(result);
        }
    }
}
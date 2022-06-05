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
    public class GetOrderByIdOrderUnit
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Success()
        {
            GetOrderByIdCommand command = new GetOrderByIdCommand()
            {
                Id = 1,
            };

            ProductInOrder productInOrder = new ProductInOrder()
            {
                Id = 1,
                Amount = 2,
                Note = ""
            };

            Order order = new Order()
            {
                Id = 1,
                Active = 1
            };

            OrderProduct orderProduct = new OrderProduct()
            {
                Amount = 2,
                Note = "no picles",
                OrderId = 1,
                ProductId = 1
            };

            IEnumerable<OrderProduct> listOrderProduct = new List<OrderProduct>()
            {
                orderProduct,
                orderProduct
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockOrderQuery = new Mock<IOrderQuery>();
            var mockProductQuery = new Mock<IProductQuery>();
            var mockOrderProductQuery = new Mock<IOrderProductQuery>();
            var mockOrderProductRepository = new Mock<IOrderProductRepository>();

            mockOrderQuery.Setup(_ => _.GetById(It.IsAny<string>())).Returns(Task.FromResult(order));
            mockOrderProductQuery.Setup(_ => _.GetAllAsync(It.IsAny<string>())).Returns(Task.FromResult(listOrderProduct));

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockOrderProductQuery.Verify(_ => _.GetAllAsync(It.IsAny<string>()), Times.Once());

            result.Should().NotBeNull();
            Assert.Equal(2, result.items.Count());
            Assert.Equal(listOrderProduct.ToList()[0].Note, result.items.ToList()[0].Note);
            Assert.Equal(listOrderProduct.ToList()[0].Amount, result.items.ToList()[0].Amount);
            Assert.Equal(listOrderProduct.ToList()[1].Note, result.items.ToList()[1].Note);
            Assert.Equal(listOrderProduct.ToList()[1].Amount, result.items.ToList()[1].Amount);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task NullResponse()
        {
            GetOrderByIdCommand command = new GetOrderByIdCommand()
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

            OrderHandler orderHandler = new OrderHandler(mockOrderRepository.Object, mockProductQuery.Object, mockOrderProductRepository.Object, mockOrderProductQuery.Object, new OrderContext(mockOrderQuery.Object));

            var result = await orderHandler.Handle(command, CancellationToken.None);

            mockOrderQuery.Verify(_ => _.GetById(It.IsAny<string>()), Times.Once());
            mockOrderProductQuery.Verify(_ => _.GetAllAsync(It.IsAny<string>()), Times.Never());

            result.Should().BeNull();
        }
    }
}
using Application.Commands.Orders;
using Application.Contexts;
using Domain.AggregatesModel.OrderAggregate;
using Domain.AggregatesModel.OrderAggregate.Interface;
using Domain.AggregatesModel.OrderProductAggregate.Interface;
using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Domain.Dtos.Order;
using Domain.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class OrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>,
                                    IRequestHandler<UpdateOrderCommand, OrderDto>,
                                    IRequestHandler<DeleteOrderCommand, bool>,
                                    IRequestHandler<GetOrderByIdCommand, Order>,
                                    IRequestHandler<CallOrderCommand, OrderDto>,
                                    IRequestHandler<GetOrderCommand, IEnumerable<Order>>
    {

        private readonly IOrderRepository _orderRepository;

        private readonly IProductQuery _productQuery;

        private readonly IOrderProductQuery _orderProductQuery;

        private readonly IOrderProductRepository _orderProductRepository;

        private readonly OrderContext _context;

        public OrderHandler(IOrderRepository orderRepository, IProductQuery productQuery, IOrderProductRepository orderProductRepository, IOrderProductQuery orderProductQuery, OrderContext context)
        {
            _orderRepository = orderRepository;
            _productQuery = productQuery;
            _orderProductQuery = orderProductQuery;
            _orderProductRepository = orderProductRepository;
            _context = context;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Order inserted = new Order();
            inserted.TableNum = request.TableNum;
            inserted.Active = 1;
            inserted.items = request.items.ToList();

            inserted.Id = await _orderRepository.InsertAsync(inserted);

            OrderProduct orderProduct = new OrderProduct();
            orderProduct.OrderId = inserted.Id;

            foreach (var item in inserted.items)
            {
                orderProduct.ProductId = item.Id;
                if (_productQuery.GetById(orderProduct.ProductId).Result != null)
                {
                    orderProduct.Amount = item.Amount;
                    orderProduct.Note = item.Note;
                    await _orderProductRepository.InsertAsync(orderProduct);
                }
                else
                {
                    throw new ArgumentException("All items must be previously registered");
                }
            }

            return inserted.ToDto();
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            Order old = await _context.GetById(request.Id.ToString());

            if (old != null)
            {
                Order updated = new Order();
                updated.Id = request.Id;
                updated.TableNum = request.TableNum;
                updated.CreatedAt = old.CreatedAt;
                updated.items = request.items.ToList();

                await _orderRepository.UpdateAsync(updated);

                await _orderProductRepository.DeleteAsync(updated.Id.ToString());

                OrderProduct orderProduct = new OrderProduct();
                orderProduct.OrderId = updated.Id;

                foreach (var item in updated.items)
                {
                    orderProduct.ProductId = item.Id; 
                    if (_productQuery.GetById(orderProduct.ProductId).Result != null)
                    {
                        orderProduct.Amount = item.Amount;
                        orderProduct.Note = item.Note;
                        await _orderProductRepository.InsertAsync(orderProduct);
                    }
                    else
                    {
                        throw new ArgumentException("All items must be previously registered");
                    }
                }

                return updated.ToDto();
            }
            return default;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != null)
            {
                await _orderRepository.DeleteAsync(request.Id);
                return true;
            }
            return default;
        }

        public async Task<IEnumerable<Order>> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _context.GetAllAsync(request.Size, request.Offset);

            if (orders.Count() != 0)
            {
                return orders;
            }

            return default;
        }

        public async Task<Order> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            Order order = await _context.GetById(request.Id.ToString());

            if (order != null)
            {
                var orderProducts = _orderProductQuery.GetAllAsync(order.Id.ToString()).Result;
                var itemList = new List<ProductInOrder>();

                foreach (var item in orderProducts)
                {
                    itemList.Add(
                        new ProductInOrder
                        {
                            Id = item.ProductId,
                            Amount = item.Amount,
                            Note = item.Note
                        }
                        );
                }
                order.items = itemList;
                return order;
            }
            return default;
        }

        public async Task<OrderDto> Handle(CallOrderCommand request, CancellationToken cancellationToken)
        {
            Order old = await _context.GetById(request.Id.ToString());
            
            if(old != null)
            {
                Order updated = new Order();
                updated.Id = request.Id;
                updated.Active = 0;

                await _orderRepository.CallUpdateAsync(updated);

                return updated.ToDto();
            }

            return default;
        }
    }
}
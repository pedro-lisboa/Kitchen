using Domain.AggregatesModel.ProductAggregate;
using System;
using System.Collections.Generic;

namespace Domain.AggregatesModel.OrderAggregate
{
    public class Order
    {
        public Order()
        {
            Active = 1;
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public int TableNum { get; set; }

        public int Active { get; set; }

        public IEnumerable<ProductInOrder> items { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
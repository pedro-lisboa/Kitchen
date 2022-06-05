using Domain.AggregatesModel.OrderAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.OrderProductAggregate.Interface
{
    public interface IOrderProductQuery
    {
        Task<IEnumerable<OrderProduct>> GetAllAsync(string orderId);
    }
}
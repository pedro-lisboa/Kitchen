using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.OrderAggregate.Interface
{
    public interface IOrderQuery
    {
        Task<IEnumerable<Order>> GetAllAsync(int size, int offset);

        Task<Order> GetById(string id);
    }
}
using Domain.AggregatesModel.OrderAggregate;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.OrderProductAggregate.Interface
{
    public interface IOrderProductRepository
    {
        Task<int> InsertAsync(OrderProduct orderProduct);

        Task UpdateAsync(OrderProduct orderProduct);

        Task DeleteAsync(string id);
    }
}
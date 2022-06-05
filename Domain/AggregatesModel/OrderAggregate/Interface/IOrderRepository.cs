using System.Threading.Tasks;

namespace Domain.AggregatesModel.OrderAggregate.Interface
{
    public interface IOrderRepository
    {
        Task<int> InsertAsync(Order order);

        Task UpdateAsync(Order order);

        Task CallUpdateAsync(Order order);

        Task DeleteAsync(string id);
    }
}
using Domain.AggregatesModel.OrderAggregate;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using Domain.AggregatesModel.OrderProductAggregate.Interface;

namespace Infrastructure.Repositories
{
    public class OrderProductRepository : IOrderProductRepository
    {
        private readonly string _connection;

        public OrderProductRepository(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<int> InsertAsync(OrderProduct orderProduct)
        {
            using IDbConnection db = new SqlConnection(_connection);

            return await db.QueryFirstOrDefaultAsync<int>(@"
                    INSERT INTO orderProducts ( 
                        orderId,
                        productId,
                        amount,
                        note
                    )  
                    VALUES (@OrderId, @ProductId, @Amount, @Note);
                    SELECT SCOPE_IDENTITY()",
                new
                {
                    orderProduct.OrderId,
                    orderProduct.ProductId,
                    orderProduct.Amount,
                    orderProduct.Note
                }
            );
        }

        public async Task UpdateAsync(OrderProduct orderProduct)
        {
            using IDbConnection db = new SqlConnection(_connection);

            await db.ExecuteAsync(@"
                    UPDATE orderProducts set ( 
                        orderId = @OrderId,
                        productId = @ProductId
                    )
                    WHERE id = @Id",
                new
                {
                    orderProduct.OrderId,
                    orderProduct.ProductId,
                    orderProduct.Id
                }
            );
        }

        public async Task DeleteAsync(string id)
        {
            using IDbConnection db = new SqlConnection(_connection);

            await db.ExecuteAsync(@"
                    DELETE FROM orderProducts 
                    WHERE OrderId = @id;",
                new
                {
                    id
                }
            );
        }
    }
}
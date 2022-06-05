using Domain.AggregatesModel.OrderAggregate;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using Domain.AggregatesModel.OrderAggregate.Interface;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connection;

        public OrderRepository(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<int> InsertAsync(Order order)
        {
            using IDbConnection db = new SqlConnection(_connection);

            return await db.QueryFirstOrDefaultAsync<int>(@"
                    INSERT INTO orders ( 
                        TableNum,
                        Active,
                        CreatedAt
                    )  
                    VALUES (@TableNum, @Active, @CreatedAt);
                    SELECT SCOPE_IDENTITY()",
                new
                {
                    order.TableNum,
                    order.Active,
                    order.CreatedAt
                }
            );
        }

        public async Task UpdateAsync(Order order)
        {
            using IDbConnection db = new SqlConnection(_connection);

            await db.ExecuteAsync(@"
                    UPDATE orders set 
                        TableNum = @TableNum
                    WHERE id = @Id",
                new
                {
                    order.TableNum,
                    order.Id
                }
            );
        }

        public async Task DeleteAsync(string id)
        {
            using IDbConnection db = new SqlConnection(_connection);

            await db.ExecuteAsync(@"
                    DELETE FROM orders 
                    WHERE id = @id;",
                new
                {
                    id
                }
            );
        }

        public async Task CallUpdateAsync(Order order)
        {
            using IDbConnection db = new SqlConnection(_connection);

            await db.ExecuteAsync(@"
                    UPDATE orders set 
                        Active = @Active
                    WHERE id = @Id",
                new
                {
                    order.Active,
                    order.Id
                }
            );
        }
    }
}
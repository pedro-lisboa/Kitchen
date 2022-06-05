using Domain.AggregatesModel.OrderAggregate;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Domain.AggregatesModel.OrderProductAggregate.Interface;

namespace Infrastructure.Queries
{
    public class OrderProductQuery : IOrderProductQuery
    {
        private readonly string _connection;

        public OrderProductQuery(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<IEnumerable<OrderProduct>> GetAllAsync(string orderId)
        {
            using IDbConnection db = new SqlConnection(_connection);

            IEnumerable<OrderProduct> result = await db.QueryAsync<OrderProduct>(@"
                    SELECT * FROM orderProducts c WHERE c.orderId = @orderId",
                    new
                    {
                        orderId
                    }
                );

            return result;
        }
    }
}
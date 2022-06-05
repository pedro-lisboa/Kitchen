using Domain.AggregatesModel.OrderAggregate;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Domain.AggregatesModel.OrderAggregate.Interface;

namespace Infrastructure.Queries
{
    public class OrderQuery : IOrderQuery
    {
        private readonly string _connection;

        public OrderQuery(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<IEnumerable<Order>> GetAllAsync(int size, int offset)
        {
            using IDbConnection db = new SqlConnection(_connection);

            IEnumerable<Order> result = await db.QueryAsync<Order>(@"
                    SELECT 
                        Id,
                        TableNum,
                        Active,
                        CreatedAt
                    FROM orders T
                    WHERE active like '1'
                    ORDER BY CreatedAt;"
                );

            return result;
        }

        public virtual async Task<Order> GetById(string id)
        {
            using IDbConnection db = new SqlConnection(_connection);

            IEnumerable<Order> result = await db.QueryAsync<Order>(@"
                    SELECT * FROM orders c WHERE c.Id = @id",
                    new
                    {
                        id
                    }
                );

            return result.FirstOrDefault();
        }
    }
}
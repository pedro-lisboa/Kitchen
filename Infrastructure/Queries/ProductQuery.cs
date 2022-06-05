using Domain.AggregatesModel.ProductAggregate;
using Domain.AggregatesModel.ProductAggregate.Interface;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly string _connection;

        public ProductQuery(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int size, int offset)
        {
            using IDbConnection db = new SqlConnection(_connection);

            IEnumerable<Product> result = await db.QueryAsync<Product>(@"
                    SELECT *
                    FROM products;"
                );

            return result;
        }

        public virtual async Task<Product> GetById(int? id)
        {
            using IDbConnection db = new SqlConnection(_connection);

            IEnumerable<Product> result = await db.QueryAsync<Product>(@"
                    SELECT * FROM products c WHERE c.id = @id",
                    new
                    {
                        id
                    }
                );

            return result.FirstOrDefault();
        }
    }
}
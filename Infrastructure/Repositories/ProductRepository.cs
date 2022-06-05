using Domain.AggregatesModel.ProductAggregate.Interface;
using Domain.AggregatesModel.ProductAggregate;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connection;

        public ProductRepository(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:kitchen"];
        }

        public async Task<int> InsertAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connection);

            return await db.QueryFirstOrDefaultAsync<int>(@"
                    INSERT INTO products ( 
                        Name,
                        Area,
                        Price
                    )  
                    VALUES (@Name, @Area, @Price);
                    SELECT SCOPE_IDENTITY()",
                new
                {
                    product.Name,
                    product.Area,
                    product.Price
                }
            );
        }
    }
}
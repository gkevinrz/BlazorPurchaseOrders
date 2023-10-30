using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorPurchaseOrders.Data
{
    public class ProductService:IProductService
    {
        //database connection
        private readonly SqlConnectionConfiguration _configuration;
        public ProductService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;    
        }

        /************************* Procs **********************/

        /* SQL Insert (create) - create a Product table row */
        public async Task<bool> ProductInsert(Product product)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                // 5 parameters
                var parameters = new DynamicParameters();
                parameters.Add("ProductCode", product.ProductCode, dbType: DbType.String);
                parameters.Add("ProductDescription", product.ProductDescription, DbType.String);
                parameters.Add("ProductUnitPrice", product.ProductUnitPrice, DbType.Decimal);
                parameters.Add("ProductSupplierID", product.ProductSupplierID, DbType.Int32);
                parameters.Add("ProductIsArchived", product.ProductIsArchived, DbType.Boolean);

                await conn.ExecuteAsync("spProduct_Insert",parameters,commandType:CommandType.StoredProcedure);
                
            }
            return true;
        }

        /* SQL Read (create) - get all rows */
        public async Task<IEnumerable<Product>> ProductList()
        {
            IEnumerable<Product> products;
           using (var conn= new SqlConnection(_configuration.Value)) {

                products = await conn.QueryAsync<Product>("spProduct_List",commandType:CommandType.StoredProcedure);
           }
           return products;
        }

        /* SQL Select (read) - Get one Product based on its ProductID  */

        public async Task<Product> Product_GetOne(int @ProductID)
        {
            Product product = new Product();  // to save product from query
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", ProductID, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                product = await conn.QueryFirstOrDefaultAsync<Product>("spProduct_GetOne", parameters, commandType: CommandType.StoredProcedure);
            }
            return product;
        }

        /* SQL Update (update) - Update one row by ProductID */
        public async Task<bool> ProductUpdate(Product product)
        {   
            var parameters = new DynamicParameters();
            parameters.Add("ProductID",product.ProductID, DbType.Int32);
            parameters.Add("ProductCode", product.ProductCode, DbType.String);
            parameters.Add("ProductDescription", product.ProductDescription, DbType.String);
            parameters.Add("ProductUnitPrice", product.ProductUnitPrice, DbType.Decimal);
            parameters.Add("ProductSupplierID", product.ProductSupplierID, DbType.Int32);
            parameters.Add("ProductIsArchived", product.ProductIsArchived, DbType.Boolean);
            using (var conn = new SqlConnection(_configuration.Value))
            {    
                // call procedure 
                await conn.ExecuteAsync("spProduct_Update",parameters,commandType: CommandType.StoredProcedure);
            }        
            return true;
        }
    }

}

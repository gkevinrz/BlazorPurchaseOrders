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
        public async Task<int> ProductInsert(
            string ProductCode,
            string ProductDescription,
            decimal ProductUnitPrice,
            int ProductSupplierID)
        {

            int Success = 0;
            var parameters = new DynamicParameters();
            parameters.Add("ProductCode", ProductCode, DbType.String);
            parameters.Add("ProductDescription", ProductDescription, DbType.String);
            parameters.Add("ProductUnitPrice", ProductUnitPrice, DbType.Decimal);
            parameters.Add("ProductSupplierID", ProductSupplierID, DbType.Int32);
            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            
            using (var conn = new SqlConnection(_configuration.Value))
            {
         
                await conn.ExecuteAsync("spProduct_Insert",parameters,commandType:CommandType.StoredProcedure);
                Success = parameters.Get<int>("@ReturnValue");

            }
            return Success;
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
        public async Task<int> ProductUpdate(
            int ProductID,
            string ProductCode,
            string ProductDescription,
            decimal ProductUnitPrice,
            int ProductSupplierID,
            bool ProductIsArchived) // Parameters
        {
            int Success = 0;
            var parameters = new DynamicParameters();
           
            parameters.Add("ProductID",ProductID, DbType.Int32);
            parameters.Add("ProductCode", ProductCode, DbType.String);
            parameters.Add("ProductDescription", ProductDescription, DbType.String);
            parameters.Add("ProductUnitPrice", ProductUnitPrice, DbType.Decimal);
            parameters.Add("ProductSupplierID", ProductSupplierID, DbType.Int32);
            parameters.Add("ProductIsArchived", ProductIsArchived, DbType.Boolean);
            // input parameter (return value)
            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using (var conn = new SqlConnection(_configuration.Value))
            {    
                // call procedure 
                await conn.ExecuteAsync("spProduct_Update",parameters,commandType: CommandType.StoredProcedure);
                // set Success var
                Success = parameters.Get<int>("@ReturnValue");
            }        
            return Success;
        }
    }

}

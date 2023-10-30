using Dapper;
using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BlazorPurchaseOrders.Data
{
    public class SupplierService: ISupplierService
    {

        //database connection
        private readonly SqlConnectionConfiguration _configuration;

        //constructor
        public SupplierService(SqlConnectionConfiguration   configuration)
        {
            _configuration = configuration;
        }


        /************************* Procs **********************/

        /* SQL Insert (create) - create a Supplier table row */
        public async Task<bool> SupplierInsert(Supplier supplier)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                // 7 parameters
                var parameters = new DynamicParameters();
                parameters.Add("SupplierName", supplier.SupplierName, DbType.String);
                parameters.Add("SupplierAddress1", supplier.SupplierAddress1, DbType.String);
                parameters.Add("SupplierAddress2", supplier.SupplierAddress2, DbType.String);
                parameters.Add("SupplierAddress3", supplier.SupplierAddress3, DbType.String);
                parameters.Add("SupplierPostCode", supplier.SupplierPostCode, DbType.String);
                parameters.Add("SupplierEmail", supplier.SupplierEmail, DbType.String);
                parameters.Add("SupplierIsArchived", supplier.SupplierIsArchived, DbType.Boolean);

                // call stored procedure
                await conn.ExecuteAsync("spSupplier_Insert", parameters, commandType: CommandType.StoredProcedure);
            }
            return true;
        }

        /* SQL Read (create) - get all rows */
        public async Task<IEnumerable<Supplier>> SupplierList()
        {
            IEnumerable<Supplier> suppliers;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                // call stored procedure
                suppliers = await conn.QueryAsync<Supplier>("spSupplier_List", commandType: CommandType.StoredProcedure);
            }
            return suppliers;
        }

        /* SQL Select (read) - Get one Supplier based on its ProductID  */
        public async Task<Supplier> Supplier_GetOne(int @SupplierID)
        {
            Supplier supplier = new Supplier();
            
            // 1 parameter
            var parameters = new DynamicParameters();
            parameters.Add("@SupplierID", SupplierID, DbType.Int32);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                supplier = await conn.QueryFirstOrDefaultAsync<Supplier>("spSupplier_GetOne", parameters, commandType: CommandType.StoredProcedure);
            }
            return supplier;
        }

        /* SQL Update (update) - Update one row by SupplierID */
        public async Task<bool> SupplierUpdate(Supplier supplier)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("SupplierID", supplier.SupplierID, DbType.Int32);

                parameters.Add("SupplierName", supplier.SupplierName, DbType.String);
                parameters.Add("SupplierAddress1", supplier.SupplierAddress1, DbType.String);
                parameters.Add("SupplierAddress2", supplier.SupplierAddress2, DbType.String);
                parameters.Add("SupplierAddress3", supplier.SupplierAddress3, DbType.String);
                parameters.Add("SupplierPostCode", supplier.SupplierPostCode, DbType.String);
                parameters.Add("SupplierEmail", supplier.SupplierEmail, DbType.String);
                parameters.Add("SupplierIsArchived", supplier.SupplierIsArchived, DbType.Boolean);

                await conn.ExecuteAsync("spSupplier_Update", parameters, commandType: CommandType.StoredProcedure);
            }
            return true;

        }



    }
}

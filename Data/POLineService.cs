using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace BlazorPurchaseOrders.Data
{
    public class POLineService:IPOLineService
    {
        // database configuration
        private readonly SqlConnectionConfiguration _configuration;

        //constructor
        public POLineService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration; 
        }

        /************************* Procs **********************/


        /* SQL Insert (create) - create a POLine table row */
        public async Task<bool> POLineInsert(POLine poline)
        {
            using (var conn =new SqlConnection(_configuration.Value))
            {
                // parameters
                var parameters= new DynamicParameters();
                // 6 parameters
                parameters.Add("POLineHeaderID", poline.POLineHeaderID, DbType.Int32);
                parameters.Add("POLineProductID", poline.POLineProductID, DbType.Int32);
                parameters.Add("POLineProductDescription", poline.POLineProductDescription, DbType.String);
                parameters.Add("POLineProductQuantity", poline.POLineProductQuantity, DbType.Decimal);
                parameters.Add("POLineProductUnitPrice", poline.POLineProductUnitPrice, DbType.Decimal);
                parameters.Add("POLineTaxRate", poline.POLineTaxRate, DbType.Decimal);

                // call store procedure
                await conn.ExecuteAsync("spPOLine_Insert", parameters, commandType: CommandType.StoredProcedure);
            }
            return true;
        }

        /* SQL Select (read) - get a list of poline  rows */
        public async Task<IEnumerable<POLine>> POLineList()
        {
            IEnumerable<POLine> polines;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                polines = await conn.QueryAsync<POLine>("spPOLine_List", commandType: CommandType.StoredProcedure);
            }
            return polines;
        }

        /* SQL Select (read) - get a poline  rows */
        public async Task<POLine> POLine_GetOne(int @POLineID) //input parameter
        {
            POLine poline = new POLine(); //save poline object

            var parameters = new DynamicParameters(); // 1 parameter
            parameters.Add("@POLineID", POLineID, DbType.Int32);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                poline = await conn.QueryFirstOrDefaultAsync<POLine>("spPOLine_GetOne", parameters, commandType: CommandType.StoredProcedure);
            }
            return poline;

        }
        

        /* SQL Update (update) - update a poline row by id */
        public async Task<bool> POLineUpdate(POLine poline)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                // parameters
                var parameters = new DynamicParameters();
                //7 parameters
                parameters.Add("POLineID", poline.POLineID, DbType.Int32);
                parameters.Add("POLineHeaderID", poline.POLineHeaderID, DbType.Int32);
                parameters.Add("POLineProductID", poline.POLineProductID, DbType.Int32);
                parameters.Add("POLineProductDescription", poline.POLineProductDescription, DbType.String);
                parameters.Add("POLineProductQuantity", poline.POLineProductQuantity, DbType.Decimal);
                parameters.Add("POLineProductUnitPrice", poline.POLineProductUnitPrice, DbType.Decimal);
                parameters.Add("POLineTaxRate", poline.POLineTaxRate, DbType.Decimal);
                // call procedure
                await conn.ExecuteAsync("spPOLine_Update", parameters, commandType: CommandType.StoredProcedure);
            }
            return true;
        }



    }
}

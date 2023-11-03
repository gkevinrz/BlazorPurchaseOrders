using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPurchaseOrders.Data
{
    public interface IPOLineService
    {
        //insert
        Task<bool> POLineInsert(POLine poline);
        //get list
        Task<IEnumerable<POLine>> POLineList();
        //get one
        Task<POLine> POLine_GetOne(int POLineID);
        // update one 
        Task<bool> POLineUpdate(POLine poline);

        Task<IEnumerable<POLine>> POLine_GetByPOHeader(int @POHeaderI);


        Task<bool> POLineDeleteOne(int @POLineID);
    }
}

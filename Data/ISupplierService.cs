using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPurchaseOrders.Data
{
    public interface ISupplierService
    {
        Task<bool> SupplierInsert(Supplier supplier);
        Task<IEnumerable<Supplier>> SupplierList();
        Task<Supplier> Supplier_GetOne(int SupplierID);
        Task<bool> SupplierUpdate(Supplier supplier);

    }
}

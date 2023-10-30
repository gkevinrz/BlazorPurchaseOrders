using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BlazorPurchaseOrders.Data
{
    public interface ITaxService
    {
        Task<int> TaxInsert(string TaxDescription, Decimal TaxRate);
        Task<IEnumerable<Tax>> TaxList();
        Task<Tax> Tax_GetOne(int TaxID);
        Task<int> TaxUpdate(string TaxDescription, Decimal TaxRate, int TaxID, bool TaxIsArchived);
    }
}

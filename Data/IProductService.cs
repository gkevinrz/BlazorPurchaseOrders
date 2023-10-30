using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPurchaseOrders.Data
{
    public interface IProductService
    {
        Task<bool> ProductInsert(Product product);
        Task<IEnumerable<Product>> ProductList();
        Task<Product> Product_GetOne(int ProductID);
        Task<bool> ProductUpdate(Product product);
    }
}

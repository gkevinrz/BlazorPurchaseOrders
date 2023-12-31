﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPurchaseOrders.Data
{
    public interface IProductService
    {
        Task<int> ProductInsert(string ProductCode,string ProductDescription,decimal ProductUnitPrice,int ProductSupplierID);
        Task<IEnumerable<Product>> ProductList();
        Task<Product> Product_GetOne(int ProductID);
        Task<int> ProductUpdate(
                int ProductID,
                string ProductCode,
                string ProductDescription,
                decimal ProductUnitPrice,
                int ProductSupplierID,
                bool ProductIsArchived
                );

        Task<IEnumerable<Product>> ProductListBySupplier(int SupplierID);

    }
}

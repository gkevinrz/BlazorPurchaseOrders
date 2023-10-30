using System;
using System.Collections.Generic; // ienumerable
using System.Linq; //
using System.Threading.Tasks; //async



namespace BlazorPurchaseOrders.Data
{
    public class SqlConnectionConfiguration
    {
        public SqlConnectionConfiguration(string value) => Value = value;
        public string Value { get; }

    }
}

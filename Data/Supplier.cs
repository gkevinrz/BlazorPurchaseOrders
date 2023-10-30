using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class Supplier
    {
        [Required]
        public int SupplierID { get; set; }
        [Required]
        [StringLength(50)]  
        public string SupplierName { get; set;}

        [StringLength(50)]
        public string SupplierAddress1 { get; set; }
        [StringLength(50)]
        public string SupplierAddress2 { get; set; }
        [StringLength(50)]
        public string SupplierAddress3 { get; set; }
        [StringLength(10)]
        public string SupplierPostCode { get; set; }
        [StringLength(256)]
        public string SupplierEmail { get; set; }
        [Required]
        public bool SupplierIsArchived { get; set; }

    }
}

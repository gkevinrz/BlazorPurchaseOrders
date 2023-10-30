using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class Product
    {
        [Required]
        public int ProductID { get; set; }
        [Required]
        [StringLength(25)]
        public string ProductCode { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductDescription { get; set; }
        [Required]
        public decimal ProductUnitPrice { get; set; }
        public int ProductSupplierID { get; set; }
        [Required]
        public bool ProductIsArchived { get; set; }
    }
}

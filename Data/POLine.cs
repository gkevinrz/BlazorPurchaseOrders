using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class POLine
    {
        [Required]
        public int POLineID { get; set; }
        [Required]
        public int POLineHeaderID { get; set; }
        [Required]
        public int POLineProductID { get; set; }
        [Required]
        [StringLength(50)]
        public string POLineProductDescription { get; set; }
        [Required]
        public decimal POLineProductQuantity { get; set; }
        [Required]
        public decimal POLineProductUnitPrice { get; set; }
        [Required]
        public decimal POLineTaxRate { get; set; }

    }
}

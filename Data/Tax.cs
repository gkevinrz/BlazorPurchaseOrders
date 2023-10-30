using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class Tax
    {
        [Required]
        public int TaxID{ get; set; }

        [Required]
        [StringLength(50)]
        public string TaxDescription { get; set; }

        [Required]
        public decimal TaxRate { get; set; }
        [Required]
        public bool TaxIsArchived { get; set; } 

    }
}

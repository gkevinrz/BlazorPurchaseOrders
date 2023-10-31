using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class Tax
    {
        [Required]
        public int TaxID{ get; set; }

        [Required(ErrorMessage ="'Description' is required")]
        [StringLength(50, ErrorMessage = "'Tax' has a maximum length of 50 characters")]
        public string TaxDescription { get; set; }

        [Required(ErrorMessage ="'Rate' is required")]
        [Range(0,1,ErrorMessage="'Rate' must be in the range 0 to 1 (0-100%)")]
        public decimal TaxRate { get; set; }
        [Required]
        public bool TaxIsArchived { get; set; } 

    }
}

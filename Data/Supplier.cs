﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorPurchaseOrders.Data
{
    public class Supplier
    {
        [Required]
        public int SupplierID { get; set; }
        [Required (ErrorMessage = "'Supplier Name' is required ")]
        [StringLength(50,MinimumLength =4,ErrorMessage ="'Supplier Name' has a minimum length of 4 and maximum of 50 characters.")]  
        public string SupplierName { get; set;}

        [StringLength(50, ErrorMessage ="'Address' has a maximum length of 50 characters")]
        public string SupplierAddress1 { get; set; }
        [StringLength(50,ErrorMessage ="'Address' has a maximum length of 50 characters")]
        public string SupplierAddress2 { get; set; }
        [StringLength(50, ErrorMessage = "'Address' has a maximum length of 50 characters")]
        public string SupplierAddress3 { get; set; }
        [StringLength(10,ErrorMessage="'Post Code' has a maximum length of 50 characters.")]
        public string SupplierPostCode { get; set; }
        [EmailAddress]
        [StringLength(256, ErrorMessage ="'Email' has a maximum length of 256 characters.")]
        public string SupplierEmail { get; set; }
        [Required]
        public bool SupplierIsArchived { get; set; }

        public string CombinedAddress { get; }


    }
}

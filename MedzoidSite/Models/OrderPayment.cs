//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedzoidSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderPayment
    {
        public int id { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public Nullable<int> orderId { get; set; }
        public Nullable<int> invoiceId { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string PaymentType { get; set; }
    }
}

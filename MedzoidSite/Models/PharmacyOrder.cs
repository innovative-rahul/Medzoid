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
    
    public partial class PharmacyOrder
    {
        public int id { get; set; }
        public int pharmacyPrescId { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public string OrderStatus { get; set; }
        public string StatusDescription { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> Isdeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public string OrderNo { get; set; }
        public Nullable<int> employeeId { get; set; }
        public string PaymentType { get; set; }
        public string OTP { get; set; }
    }
}

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
    
    public partial class spGetPharmacyOrders_Result
    {
        public int PrecriptionId { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> employeeId { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public string OrderNo { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string DeliveryType { get; set; }
        public string User_Address { get; set; }
        public string latlong { get; set; }
        public string otp { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}

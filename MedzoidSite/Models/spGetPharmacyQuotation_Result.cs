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
    
    public partial class spGetPharmacyQuotation_Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string PharmacyType { get; set; }
        public string Description { get; set; }
        public string ApprovalDescription { get; set; }
        public string QuotationDescription { get; set; }
        public string PrescriptionEditDescription { get; set; }
        public Nullable<System.DateTime> PrescriptionDate { get; set; }
        public string IsApproved { get; set; }
        public string PrescriptionStatus { get; set; }
        public string User_Address { get; set; }
        public string latlong { get; set; }
        public string DeliveryType { get; set; }
    }
}

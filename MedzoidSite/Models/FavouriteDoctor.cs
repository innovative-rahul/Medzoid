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
    
    public partial class FavouriteDoctor
    {
        public int id { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> DoctorId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public string UserType { get; set; }
        public Nullable<int> ClinicId { get; set; }
    
        public virtual ClinicDetail ClinicDetail { get; set; }
        public virtual UserMaster UserMaster { get; set; }
        public virtual UserMaster UserMaster1 { get; set; }
    }
}
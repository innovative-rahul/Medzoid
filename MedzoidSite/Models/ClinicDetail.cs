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
    
    public partial class ClinicDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClinicDetail()
        {
            this.clinicSchedules = new HashSet<clinicSchedule>();
            this.doc_employee_master = new HashSet<doc_employee_master>();
            this.FavouriteDoctors = new HashSet<FavouriteDoctor>();
            this.make_appointment = new HashSet<make_appointment>();
        }
    
        public int id { get; set; }
        public Nullable<int> doc_id { get; set; }
        public string hospital_name { get; set; }
        public string hospital_address { get; set; }
        public string contact_details { get; set; }
        public string latlog { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual UserMaster UserMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clinicSchedule> clinicSchedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<doc_employee_master> doc_employee_master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavouriteDoctor> FavouriteDoctors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<make_appointment> make_appointment { get; set; }
    }
}

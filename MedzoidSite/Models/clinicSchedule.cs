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
    
    public partial class clinicSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clinicSchedule()
        {
            this.make_appointment = new HashSet<make_appointment>();
        }
    
        public int id { get; set; }
        public Nullable<int> docId { get; set; }
        public Nullable<int> clinicId { get; set; }
        public string weekday { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> Isdeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ClinicDetail ClinicDetail { get; set; }
        public virtual UserMaster UserMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<make_appointment> make_appointment { get; set; }
    }
}

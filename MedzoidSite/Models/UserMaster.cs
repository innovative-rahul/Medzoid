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
    
    public partial class UserMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserMaster()
        {
            this.BloodDonorDetails = new HashSet<BloodDonorDetail>();
            this.BloodDonorDetails1 = new HashSet<BloodDonorDetail>();
            this.ClinicDetails = new HashSet<ClinicDetail>();
            this.clinicSchedules = new HashSet<clinicSchedule>();
            this.doc_employee_master = new HashSet<doc_employee_master>();
            this.DoctorMasters = new HashSet<DoctorMaster>();
            this.make_appointment = new HashSet<make_appointment>();
            this.PharmacyHours = new HashSet<PharmacyHour>();
            this.PharmacyMasters = new HashSet<PharmacyMaster>();
            this.Prescriptions = new HashSet<Prescription>();
            this.FavouriteDoctors = new HashSet<FavouriteDoctor>();
            this.FavouriteDoctors1 = new HashSet<FavouriteDoctor>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string photo { get; set; }
        public string blood_group { get; set; }
        public string blood_donate { get; set; }
        public string user_type { get; set; }
        public string password { get; set; }
        public string latlong { get; set; }
        public string gcmkey { get; set; }
        public string OTP { get; set; }
        public string used_ref { get; set; }
        public string ref_code { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodDonorDetail> BloodDonorDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodDonorDetail> BloodDonorDetails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClinicDetail> ClinicDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clinicSchedule> clinicSchedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<doc_employee_master> doc_employee_master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoctorMaster> DoctorMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<make_appointment> make_appointment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacyHour> PharmacyHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacyMaster> PharmacyMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavouriteDoctor> FavouriteDoctors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavouriteDoctor> FavouriteDoctors1 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class AfterLoginViewModel
    {
        public UserMasterVM userDetails { get; set; }
        public DoctorViewModel DoctorDetails { get; set; }
        public List<Department> Departments { get; set; }

    }

    public class UserMasterVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string BloodDonor { get; set; }

        public string LatLong { get; set; }
        public string Address { get; set; }
    }

    public class DoctorViewModel
    {
        public int docId { get; set; }
        public string Name { get; set; }
        public string MciNo { get; set; }
        public string AboutUs { get; set; }
        public string OnlineAppointment { get; set; }
        public string deptId { get; set; }
        public string experience { get; set; }
        public string Fee { get; set; }
        public string HospitalAffiliation { get; set; }
        public string Membership { get; set; }
    }

    public class Department 
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public string deptType { get; set; }
    }
}
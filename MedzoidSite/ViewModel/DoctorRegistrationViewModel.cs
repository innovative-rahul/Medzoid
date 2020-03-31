using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class DoctorRegistrationViewModel
    {
        public List<DoctorMaster> UnclaimeDoctors { get; set; }
        public DoctorRegistrationViewModelInput Doctor { get; set; }
    }

    public class DoctorRegistrationViewModelInput : UserMaster 
    {
        public string mci_No { get; set; }
        public string AboutUs { get; set; }
        public string OnLineAppointment { get; set; }
        public string deptId { get; set; }
        public string experience { get; set; }
        public string Fee { get; set; }
        public string HosptalAffiliation { get; set; }
        public string Membership { get; set; }
        public string Locality { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class Doctor
    {
        public int doc_id { get; set; }
        public string DocName { get; set; }
        public string mciNo { get; set; }
        public string AboutUs { get; set; }
        public string OnlineAppointment { get; set; }
        public string dept_id { get; set; }
        public string deptName { get; set; }
        public string experience { get; set; }
        public string Fee { get; set; }
        public string LatLong { get; set; }
        public string Photo { get; set; }
        public string HospitalAffiliation { get; set; }
        public string Membership { get; set; }
        public string Qualification { get; set; }
        public int clinicId { get; set; }
        public string HospitalName { get; set; }
        public string ContactNumber { get; set; }
        public string HospitalAddress { get; set; }
        public double? Rating { get; set; }
        public int RatingByCount { get; set; }
        public bool IsSufficientWalletBalance { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsApproved { get; set; }

    }


    public class DoctorPractiseViewModel
    {
        public int TotalAppointments { get; set; }
        public int TotalOffline { get; set; }
        public int TotalOnline { get; set; }
        public List<ClinicsDetails> clinics { get; set; }
        public List<TimeSchedule> timeschedule { get; set; }
    }


    public class ClinicsDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class TimeSchedule
    {
        public int Id { get; set; }
        public string Time { get; set; }
    }

}
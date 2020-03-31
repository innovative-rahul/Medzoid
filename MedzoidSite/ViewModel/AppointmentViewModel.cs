using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? docId { get; set; }
        public string DocName { get; set; }
        public int? ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public string Date { get; set; }
        public string AppointmentStatus { get; set; }
        public Int64? QueueNo { get; set; }
        public string PatientName { get; set; }
        public int? PatientAge { get; set; }
        public string PatientSex { get; set; }
        public string AppointmentSchedule { get; set; }
        public string AppointmentType { get; set; }
        public bool ReviewMade { get; set; }
        public string AppointmentNo { get; set; }
    }

    public class TrackAppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string AppointmentNo { get; set; }
        public string Date { get; set; }
        public string DoctorName { get; set; }
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
        public string scheduleTime { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string Latlong { get; set; }
        public string PatientName { get; set; }

        public string Status { get; set; }
        public List<AppointmentList> appointmentList { get; set; }
    }

    public class AppointmentList
    {
        public Int64? AppointmentNo { get; set; }
        public string Status { get; set; }
    }
}
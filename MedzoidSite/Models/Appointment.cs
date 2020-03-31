using MedzoidSite.Models;
using MedzoidSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class Appointment
    {
        private MedzoidEntities db = new MedzoidEntities();
        public DoctorPractiseViewModel GetAppointmentCount(DoctorPractiseParam param)
        {
            DoctorPractiseViewModel model = new DoctorPractiseViewModel();
            var appointments = db.make_appointment.Where(a => a.doctor_id == param.Id).ToList();
            if (appointments.Count > 0)
            {
                if(param.ClinicId != 0)
                {
                  appointments = appointments.Where(a => a.clinic_id == param.ClinicId).ToList();
                }
                if (param.ScheduleId != 0)
                {
                  appointments = appointments.Where(a => a.scheduleId == param.ScheduleId).ToList();
                }
                if (param.Month != 0)
                {
                  appointments = appointments.Where(a => a.date.Value.Month == param.Month).ToList();
                }
                if (param.Year != 0)
                {
                    appointments = appointments.Where(a => a.date.Value.Year == param.Year).ToList();
                }

                if (param.AppointmentStatus != null && param.AppointmentStatus != "0")
                {
                  appointments = appointments.Where(a => a.AppointmentStatus == param.AppointmentStatus).ToList();
                }

                model.TotalAppointments = appointments.Count;
                model.TotalOffline = appointments.Where(a => a.AppointmentType == "2").Count();
                model.TotalOnline = appointments.Where(a => a.AppointmentType == "1").Count();
            }

            return model;
        }
    }



    public class DoctorPractiseParam
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int ScheduleId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string AppointmentStatus { get; set; }
    }
}
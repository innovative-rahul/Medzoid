using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class ClinicScheduleViewModel
    {
        public List<clinicSchedule> scheduleList { get; set; }
        public clinicSchedule schedule { get; set; }
    }
}
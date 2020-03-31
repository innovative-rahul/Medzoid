using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class ClinicDetailsViewModel
    {
        public ClinicDetail Clinic { get; set; }
        public List<ClinicDetail> ClinicList { get; set; }
       
    }
}
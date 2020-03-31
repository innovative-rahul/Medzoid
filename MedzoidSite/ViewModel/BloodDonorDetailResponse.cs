using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class BloodDonorDetailResponse
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public string patient_name { get; set; }
        public Nullable<int> age { get; set; }
        public string gender { get; set; }
        public string doctor_name { get; set; }
        public string hospital_name { get; set; }
        public string when_required { get; set; }
        public Nullable<int> doctor_id { get; set; }
        public string RequestedPersonName { get; set; }
        public string ReqTo { get; set; }
        public string bloodGroup { get; set; }
        public string ReqStatus { get; set; }
        public string CreationDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
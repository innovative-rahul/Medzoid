using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class UserDetails
    {
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
    }
}
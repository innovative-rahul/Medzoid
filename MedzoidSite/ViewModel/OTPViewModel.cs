using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class OTPViewModel
    {
        public int Id { get; set; }
        public string referal_amount { get; set; }
        public string wallet_amount { get; set; }
        public string Msg { get; set; }
        public string Ref_Msg { get; set; }
    }

    public class OTPInputModel
    {
        public string otp { get; set; }
        public string usedrefcode { get; set; }
        public int Id { get; set; }
    }
}
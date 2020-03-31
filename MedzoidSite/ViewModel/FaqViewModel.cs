using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class FaqViewModel :FAq
    {
        public FAq Faq { get; set; }

        public List<FAq> FaqList { get; set; }
    }



}
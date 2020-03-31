using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class HealthTipsViewModel
    {
        public HealthTip HealthTips { get; set; }

        public List<HealthTip> HealthTipsList { get; set; }
    }
}
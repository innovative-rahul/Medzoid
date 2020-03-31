using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class PrescriptionViewModel
    {
        public List<Presc> PrescList { get; set; }
    }


    public class Presc
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string DeliveryType { get; set; }
        public DateTime? Createddate { get; set; }

    }
}
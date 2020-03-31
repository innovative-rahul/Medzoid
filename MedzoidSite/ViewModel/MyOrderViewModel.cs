using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class MyOrderViewModel
    {
        public string OrderNo { get; set; }
        public decimal? OrderAmount { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderDescription { get; set; }
        public string PaymentMode { get; set; }
        public string PharmacyAddress { get; set; }
        public string DeliveryType { get; set; }
        public string PaymentStatus { get; set; }
    }
}
namespace MedzoidSite.ViewModel
{
    public class OrderViewModel
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
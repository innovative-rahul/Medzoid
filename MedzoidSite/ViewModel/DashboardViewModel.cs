namespace MedzoidSite.ViewModel
{
    public class DashboardViewModel
    {
       
            public int Orders { get; set; }
            public int WalletAmount { get; set; }
            public int Appointment { get; set; }
            public int FavDoctors { get; set; }
       
    }


    public class AdminDashboardViewModel
    {
        public int Orders { get; set; }
        public int Users { get; set; }
        public int Doctors { get; set; }
        public int Prescriptions { get; set; }
    }

    public class PharmacyDashboardViewModel
    {
        public int Orders { get; set; }
        public int Prescriptions { get; set; }
        public int DuePayment { get; set; }
        public int DueFecilitationCharges { get; set; }
    }

}
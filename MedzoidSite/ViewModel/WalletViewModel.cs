using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class WalletViewModel
    {
        public List<Wallet> walletDetails { get; set; }

        public decimal TotalAvailableAmount { get; set; }

        public int Amount { get; set; }

    }



    public class Wallet
    {
        public int? userId { get; set; }

        [Required]
        [Range(50, Int32.MinValue)]
        public decimal amount { get; set; }
        public string TransferType { get; set; }
        public string TransferDate { get; set; }
        public string Message { get; set; }
    }


}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeWallet_Mobile_.Data.Models
{
    public class UsersReceipts
    {
        public string id { get; set; }
        public string ShopId { get; set; }
        public byte[] ShopImage { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalAmount { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_.Data.DTO_s.Json
{
    public class UsersReceiptsDTO
    {
        public string id { get; set; }
        public string ShopId { get; set; }
        public byte[] ShopImage { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalAmount { get; set; }
        public string UserId { get; set; }
    }
}

using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.Models
{
    public class Receipt
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string ShopId { get; set; }

        [Required]
        public byte[] ShopImage { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

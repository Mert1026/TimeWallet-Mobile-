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
    public class ReceiptItem
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string ReceiptId { get; set; }

        [Required]
        [ForeignKey(nameof(ReceiptId))]
        public Receipt Receipts { get; set; }
    }
}

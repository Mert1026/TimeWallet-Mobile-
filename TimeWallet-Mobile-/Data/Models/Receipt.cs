﻿using Microsoft.Maui.Controls;
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
        public string id { get; set; }

        [Required]
        public string ShopId { get; set; }

        [Required]
        public byte[] ShopImage { get; set; }
        [Required]
        public DateTime createdAt { get; set; }

        [Required]
        public double TotalAmount { get; set; }

    }
}

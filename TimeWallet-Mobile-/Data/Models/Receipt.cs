using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TimeWallet_Mobile_.Data.Models
{
    public class Receipt
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("shopId")]
        public string ShopId { get; set; }

        [JsonPropertyName("shopImage")]
        public string ShopImage { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonPropertyName("totalAmount")]
        public double TotalAmount { get; set; }

    }
}

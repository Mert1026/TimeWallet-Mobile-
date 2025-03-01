using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.Models
{
    public class Elements
    {
        [JsonPropertyName("id")]
        public Guid id { get; set; }

        [JsonPropertyName("budgetId")]
        public Guid BudgetId { get; set; }

        [JsonPropertyName("budgets")]
        public Budgets Budgets { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("createdAt")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("receiptId")]
        public int? ReceiptId { get; set; }
    }
}

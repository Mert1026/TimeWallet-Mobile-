using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.DTO_s.Json
{
    public class ElementDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public long createdAt { get; set; }
        public decimal amount { get; set; }
        public Guid budgetId { get; set; }
        //isValid() func - Приложи
        public string? receiptId { get; set; }
    }
}

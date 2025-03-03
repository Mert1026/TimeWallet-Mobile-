using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.DTO_s.Json
{
    public class BudgetAddDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long CreatedAt { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_.Data.DTO_s.Json
{
    public class ListOfBudgetsAndElements
    {
        public List<Budgets> Budgets { get; set; }
        public List<Elements> Elements { get; set; }

    }
}

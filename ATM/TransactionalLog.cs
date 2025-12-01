using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace ATM
{
    public class TransactionLog
    {
        public string PersonalId { get; set; } = "";
        public string UserFullName { get; set; } = "";
        public string Operation { get; set; } = "";
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; } = "";
    }
}

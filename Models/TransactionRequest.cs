using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanca.Models
{
    public class TransactionRequest
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime? Date { get; set; }
    }
}
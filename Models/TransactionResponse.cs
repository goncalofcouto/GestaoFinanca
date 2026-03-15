using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanca.Models
{
    public class TransactionResponse
    {

        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
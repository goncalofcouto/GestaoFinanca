using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanca.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero!")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
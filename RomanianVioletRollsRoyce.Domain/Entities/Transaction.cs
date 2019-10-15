using System;

namespace RomanianVioletRollsRoyce.Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }
    }
}

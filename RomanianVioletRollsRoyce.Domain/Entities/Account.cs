using System.Collections.Generic;

namespace RomanianVioletRollsRoyce.Domain.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        public int CustomerId { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using RomanianVioletRollsRoyce.Domain.Entities;

namespace RomanianVioletRollsRoyce.Data.DTO
{
    public class CustomerAccountData
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int AccountId { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

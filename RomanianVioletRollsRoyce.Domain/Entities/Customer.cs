using System.Collections.Generic;

namespace RomanianVioletRollsRoyce.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}

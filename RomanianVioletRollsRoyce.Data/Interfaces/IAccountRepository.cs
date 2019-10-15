using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Interfaces
{
    public interface IAccountRepository : IRepository
    {
        Task<Account> GetAccount(int accountId);

        Task<ICollection<Account>> GetAllAccounts();

        Task<Account> CreateAccount(int customerId);
    }
}

using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Interfaces
{
    public interface ITransactionRepository : IRepository
    {
        Task<ICollection<Transaction>> GetAllTransactions();

        Task<Transaction> CreateTransaction(int accountId, decimal amount);
    }
}

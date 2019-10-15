using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Domain.Entities;

namespace RomanianVioletRollsRoyce.Services.Interfaces
{
    public interface ITransactionService : IService
    {
        Task<Transaction> CreateTransaction(int accountId, decimal amount);
    }
}

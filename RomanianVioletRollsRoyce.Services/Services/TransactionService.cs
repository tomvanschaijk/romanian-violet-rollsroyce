using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using RomanianVioletRollsRoyce.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ILogger<TransactionService> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task<Transaction> CreateTransaction(int accountId, decimal amount)
        {
            _logger.LogInformation($"Creating new transaction with amount {amount} for account with id {accountId}");

            var transaction = await _transactionRepository.CreateTransaction(accountId, amount);
            return transaction;
        }
    }
}

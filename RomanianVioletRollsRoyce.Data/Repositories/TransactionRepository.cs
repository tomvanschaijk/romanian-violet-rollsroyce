using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Repositories
{
    public class TransactionRepository : DataRepositoryBase, ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly IAccountRepository _accountRepository;

        public TransactionRepository(ILogger<TransactionRepository> logger, IMemoryCache cache, IAccountRepository accountRepository) : base(cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ICollection<Transaction>> GetAllTransactions() => await GetAll<Transaction>('T');

        public async Task<Transaction> CreateTransaction(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccount(accountId);

            var transactions = await GetAllTransactions();
            var transactionId = transactions.Select(e => e.TransactionId).OrderBy(e => e).LastOrDefault();
            transactionId++;
            var transaction = new Transaction
            {
                TransactionId = transactionId,
                AccountId = account.AccountId,
                Amount = amount,
                DateTime = DateTime.Now
            };

            account.Balance += transaction.Amount;
            account.Transactions.Add(transaction);

            var transactionKey = $"T-{accountId}";
            _cache.Set(transactionKey, transaction, TimeSpan.FromHours(1));

            return transaction;
        }
    }
}

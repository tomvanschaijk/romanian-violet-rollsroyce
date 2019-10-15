using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Exceptions;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Repositories
{
    public class AccountRepository : DataRepositoryBase, IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(ILogger<AccountRepository> logger, IMemoryCache cache) : base(cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Account> GetAccount(int accountId)
        {
            var account = _cache.Get<Account>($"A-{accountId}");
            if (account == null)
            {
                _logger.LogError($"Account with id '{accountId}' not found");
                throw new DataNotFoundException("Requested account not found!");
            }

            return account;
        }

        public async Task<ICollection<Account>> GetAllAccounts() => await GetAll<Account>('A');

        public async Task<Account> CreateAccount(int customerId)
        {
            try
            {
                var customerKey = $"C-{customerId}";
                var customer = _cache.Get<Customer>(customerKey);
                if (customer == null)
                {
                    _logger.LogError($"Customer with id '{customerId}' not found");
                    throw new DataNotFoundException("Requested customer not found!");
                }

                var accounts = await GetAllAccounts();
                var accountId = accounts.Select(e => e.AccountId).OrderBy(e => e).LastOrDefault();
                accountId++;
                var account = new Account
                {
                    AccountId = accountId,
                    CustomerId = customerId
                };

                customer.Accounts.Add(account);
                _cache.Set(customerKey, customer, TimeSpan.FromHours(1));

                var accountKey = $"A-{accountId}";
                _cache.Set(accountKey, account, TimeSpan.FromHours(1));

                return account;
            }
            catch (DataNotFoundException) { throw; }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Exception occurred during account creation: {@Exception}", exc);
                throw;
            }
        }
    }
}

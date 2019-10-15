using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Data.DTO;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using RomanianVioletRollsRoyce.Domain.Requests;
using RomanianVioletRollsRoyce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public AccountService(ILogger<AccountService> logger, IServiceFactory serviceFactory, IRepositoryFactory repositoryFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public async Task<Account> CreateAccount(CreateAccountRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            _logger.LogInformation("Creating account for incoming request {@Request}", request);

            var repos = _repositoryFactory.GetRepository<IAccountRepository>();
            var account = await repos.CreateAccount(request.CustomerId);

            if (request.InitialCredit != 0)
            {
                await _serviceFactory.GetService<ITransactionService>()
                                     .CreateTransaction(account.AccountId, request.InitialCredit);
            }

            return account;
        }

        public async Task<ICollection<CustomerAccountData>> GetCustomerAccountData()
        {
            _logger.LogInformation("Getting customer account data");

            var customers = await _repositoryFactory.GetRepository<ICustomerRepository>().GetAllCustomers();
            var customerAccountData = customers.OrderBy(e => e.Surname)
                                               .ThenBy(e => e.Name)
                                               .SelectMany(customer => customer.Accounts.Select(account => new CustomerAccountData
                                               {
                                                   Name = customer.Name,
                                                   Surname = customer.Surname,
                                                   AccountId = account.AccountId,
                                                   Balance = account.Balance,
                                                   Transactions = account.Transactions.OrderBy(e => e.DateTime).ToList()
                                               })).ToList();

            return customerAccountData;
        }
    }
}

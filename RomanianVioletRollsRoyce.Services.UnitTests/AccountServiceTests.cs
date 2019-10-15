using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using RomanianVioletRollsRoyce.Domain.Requests;
using RomanianVioletRollsRoyce.Services.Interfaces;
using RomanianVioletRollsRoyce.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Services.UnitTests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private AccountService _sut;
        private readonly ILogger<AccountService> _logger = new NullLogger<AccountService>();
        private readonly Mock<IServiceFactory> _serviceFactory = new Mock<IServiceFactory>();
        private readonly Mock<IRepositoryFactory> _repositoryFactory = new Mock<IRepositoryFactory>();

        [SetUp]
        public void Setup()
        {
            _serviceFactory.Reset();
            _repositoryFactory.Reset();
            _sut = new AccountService(_logger, _serviceFactory.Object, _repositoryFactory.Object);
        }

        [Test]
        public void Constructor_With_NullParameters_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AccountService(null, _serviceFactory.Object, _repositoryFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new AccountService(_logger, null, _repositoryFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new AccountService(_logger, _serviceFactory.Object, null));
            Assert.Throws<ArgumentNullException>(() => new AccountService(null, null, null));
        }

        [Test]
        public async Task CreateAccount_With_NullRequest_Should_Throw_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAccount(null));
        }

        [Test]
        public async Task CreateAccount_With_ValidRequest_Should_Call_Repository()
        {
            var expectedAccount = new Account { CustomerId = 1, AccountId = 1, Balance = 0 };
            var request = new CreateAccountRequest { CustomerId = 1, InitialCredit = 0 };
            _repositoryFactory.Setup(e => e.GetRepository<IAccountRepository>().CreateAccount(request.CustomerId))
                              .ReturnsAsync(expectedAccount);

            var account = await _sut.CreateAccount(request);

            _repositoryFactory.Verify(e => e.GetRepository<IAccountRepository>().CreateAccount(request.CustomerId), Times.Once);
            _serviceFactory.Verify(e => e.GetService<ITransactionService>().CreateTransaction(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
            Assert.AreEqual(expectedAccount.CustomerId, account.CustomerId);
            Assert.AreEqual(expectedAccount.AccountId, account.AccountId);
            Assert.AreEqual(expectedAccount.Balance, account.Balance);
            Assert.IsEmpty(account.Transactions);
        }

        [Test]
        public async Task CreateAccount_With_InitialCreditNotZero_Should_CreateTransaction()
        {
            var expectedAccount = new Account
            {
                CustomerId = 1,
                AccountId = 1,
                Balance = 100,
                Transactions = new List<Transaction> { new Transaction { AccountId = 1, Amount = 100 } }
            };
            var request = new CreateAccountRequest { CustomerId = 1, InitialCredit = 100 };
            _repositoryFactory.Setup(e => e.GetRepository<IAccountRepository>().CreateAccount(request.CustomerId))
                              .ReturnsAsync(expectedAccount);
            _serviceFactory.Setup(e =>e.GetService<ITransactionService>().CreateTransaction(It.IsAny<int>(), It.IsAny<decimal>()))
                           .ReturnsAsync(expectedAccount.Transactions.FirstOrDefault());

            var account = await _sut.CreateAccount(request);

            _repositoryFactory.Verify(e => e.GetRepository<IAccountRepository>().CreateAccount(request.CustomerId), Times.Once);
            _serviceFactory.Verify(e => e.GetService<ITransactionService>().CreateTransaction(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
            Assert.AreEqual(expectedAccount.CustomerId, account.CustomerId);
            Assert.AreEqual(expectedAccount.AccountId, account.AccountId);
            Assert.AreEqual(expectedAccount.Balance, account.Balance);
            Assert.AreEqual(1, account.Transactions.Count);
            Assert.AreEqual(expectedAccount.Transactions.FirstOrDefault().AccountId, account.Transactions.FirstOrDefault().AccountId);
            Assert.AreEqual(expectedAccount.Transactions.FirstOrDefault().Amount, account.Transactions.FirstOrDefault().Amount);
        }
    }
}
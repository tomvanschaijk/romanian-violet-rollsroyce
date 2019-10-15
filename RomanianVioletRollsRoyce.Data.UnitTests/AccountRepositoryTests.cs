using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using RomanianVioletRollsRoyce.Crosscutting.Exceptions;
using RomanianVioletRollsRoyce.Data.Repositories;
using RomanianVioletRollsRoyce.Data.UnitTests;
using System;

namespace RomanianVioletRollsRoyce.Services.UnitTests
{
    [TestFixture]
    public class AccountRepositoryTests
    {
        private AccountRepository _sut;
        private readonly ILogger<AccountRepository> _logger = new NullLogger<AccountRepository>();
        private Mock<IMemoryCache> _cache = new Mock<IMemoryCache>();

        [SetUp]
        public void Setup()
        {
            _cache.Reset();
            _sut = new AccountRepository(_logger, _cache.Object);
        }

        [Test]
        public void Constructor_With_NullParameters_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AccountRepository(null, _cache.Object));
            Assert.Throws<ArgumentNullException>(() => new AccountRepository(_logger, null));
            Assert.Throws<ArgumentNullException>(() => new AccountRepository(null, null));
        }

        [Test]
        public void GetAccount_With_Unknown_AccountId_Should_Throw_DataNotFoundException()
        {
            _cache = MockMemoryCache.CreateMemoryMock(null);
            Assert.ThrowsAsync<DataNotFoundException>(() => _sut.GetAccount(It.IsAny<int>()));
        }
    }
}
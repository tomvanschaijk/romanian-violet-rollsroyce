using RomanianVioletRollsRoyce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Data.DTO;
using RomanianVioletRollsRoyce.Domain.Requests;

namespace RomanianVioletRollsRoyce.Services.Interfaces
{
    public interface IAccountService : IService
    {
        Task<Account> CreateAccount(CreateAccountRequest request);

        Task<ICollection<CustomerAccountData>> GetCustomerAccountData();
    }
}

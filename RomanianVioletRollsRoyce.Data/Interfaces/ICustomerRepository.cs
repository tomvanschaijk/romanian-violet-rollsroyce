using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Interfaces
{
    public interface ICustomerRepository : IRepository
    {
        Task<Customer> GetCustomer(int customerId);

        Task<ICollection<Customer>> GetAllCustomers();
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace RomanianVioletRollsRoyce.Crosscutting.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepository
            => _serviceProvider.GetService<TRepository>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace RomanianVioletRollsRoyce.Crosscutting.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TService GetService<TService>() where TService : IService
            => _serviceProvider.GetService<TService>();
    }
}

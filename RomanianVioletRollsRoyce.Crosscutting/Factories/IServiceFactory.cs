namespace RomanianVioletRollsRoyce.Crosscutting.Factories
{
    public interface IServiceFactory
    {
        TService GetService<TService>() where TService : IService;
    }
}

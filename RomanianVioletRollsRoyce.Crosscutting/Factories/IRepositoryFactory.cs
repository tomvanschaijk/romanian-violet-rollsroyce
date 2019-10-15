namespace RomanianVioletRollsRoyce.Crosscutting.Factories
{
    public interface IRepositoryFactory
    {
        TRepository GetRepository<TRepository>() where TRepository : IRepository;
    }
}

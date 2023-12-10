namespace DataLayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}

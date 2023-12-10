namespace DataLayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; set; }
        IProjectsRepository ProjectsRepository { get; set; }
        void Save();
    }
}

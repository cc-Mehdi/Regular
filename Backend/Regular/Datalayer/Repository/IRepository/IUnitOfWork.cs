namespace DataLayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; set; }
        IProjectsRepository ProjectsRepository { get; set; }
        ITasksRepository TasksRepository { get; set; }
        IFriendsRepository FriendsRepository { get; set; }
        void Save();
    }
}

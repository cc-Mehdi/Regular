namespace Datalayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; set; }
        IOrganizationsRepository OrganizationsRepository { get; set; }
        IProjectsRepository ProjectsRepository { get; set; }
        IUser_ProjectRepository User_ProjectRepository { get; set; }
        ITasksRepository TasksRepository { get; set; }
        ILoginsLogRepository LoginsLogRepository { get; set; }
        IUsers_UsersRepository Users_UsersRepository { get; set; }
        IOrganizations_UsersRepository Organizations_UsersRepository { get; set; }
        void Save();
    }
}

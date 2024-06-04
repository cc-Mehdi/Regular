namespace Datalayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; set; }
        IOrganizationsRepository OrganizationsRepository { get; set; }
        IProjectsRepository ProjectsRepository { get; set; }
    IEmployeeInvitesRepository EmployeeInvitesRepository { get; set; }
        IOrganization_ProjectRepository Organization_ProjectRepository { get; set; }
        IUser_ProjectRepository User_ProjectRepository { get; set; }
        ITasksRepository TasksRepository { get; set; }
        ILoginsLogRepository LoginsLogRepository { get; set; }
        void Save();
    }
}

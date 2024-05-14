using Datalayer.Data;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UsersRepository = new UsersRepository(_db);
            OrganizationsRepository = new OrganizationsRepository(_db);
            ProjectsRepository = new ProjectsRepository(_db);
            EmployeeInvitesRepository = new EmployeeInvitesRepository(_db);
            Organization_ProjectRepository = new Organization_ProjectRepository(_db);
            User_ProjectRepository = new User_ProjectRepository(_db);
            TasksRepository = new TasksRepository(_db);
        }

        public IUsersRepository UsersRepository { get; set; }
        public IOrganizationsRepository OrganizationsRepository { get; set; }
        public IProjectsRepository ProjectsRepository { get; set; }
        public IEmployeeInvitesRepository EmployeeInvitesRepository { get; set; }
        public IOrganization_ProjectRepository Organization_ProjectRepository { get; set; }
        public IUser_ProjectRepository User_ProjectRepository { get; set; }
        public ITasksRepository TasksRepository { get; set; }


        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

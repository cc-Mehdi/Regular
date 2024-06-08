using Datalayer.Data;
using Datalayer.Models;
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
            User_ProjectRepository = new User_ProjectRepository(_db);
            TasksRepository = new TasksRepository(_db);
            Users_UsersRepository = new Users_UsersRepository(_db);
            Organizations_UsersRepository = new Organizations_UsersRepository(_db);
            LoginsLogRepository = new LoginsLogRepository(_db);
        }

        public IUsersRepository UsersRepository { get; set; }
        public IOrganizationsRepository OrganizationsRepository { get; set; }
        public IProjectsRepository ProjectsRepository { get; set; }
        public IUser_ProjectRepository User_ProjectRepository { get; set; }
        public ITasksRepository TasksRepository { get; set; }
        public ILoginsLogRepository LoginsLogRepository { get; set; }
        public IUsers_UsersRepository Users_UsersRepository { get; set; }
        public IOrganizations_UsersRepository Organizations_UsersRepository { get; set; }


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

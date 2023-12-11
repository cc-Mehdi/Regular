using DataLayer.Data;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UsersRepository = new UsersRepository(_db);
            ProjectsRepository = new ProjectsRepository(_db);
            TasksRepository = new TasksRepository(_db);
            FriendsRepository = new FriendsRepository(_db);
        }

        public IUsersRepository UsersRepository { get; set; }
        public IProjectsRepository ProjectsRepository { get; set; }
        public ITasksRepository TasksRepository { get; set; }
        public IFriendsRepository FriendsRepository { get; set; }

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

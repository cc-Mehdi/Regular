using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class User_ProjectRepository : Repository<User_Project>, IUser_ProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public User_ProjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User_Project user_Project)
        {
            var objFromDb = _db.User_Project.FirstOrDefault(u => u.Id == user_Project.Id);
            objFromDb.User = user_Project.User;
            objFromDb.UserId = user_Project.UserId;
            objFromDb.Project = user_Project.Project;
            objFromDb.ProjectId = user_Project.ProjectId;
        }
    }
}

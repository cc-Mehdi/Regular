using DataLayer.Data;
using DataLayer.Models;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
{
    public class ProjectsRepository : Repository<Projects>, IProjectsRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Projects project)
        {
            var objFromDb = _db.Projects.FirstOrDefault(u => u.Id == project.Id);
            objFromDb.ProjectName = project.ProjectName;
        }
    }
}

using DataLayer.Data;
using DataLayer.Models;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
{
    public class TasksRepository : Repository<Tasks>, ITasksRepository
    {
        private readonly ApplicationDbContext _db;
        public TasksRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Tasks task)
        {
            var objFromDb = _db.Tasks.FirstOrDefault(u => u.Id == task.Id);
            objFromDb.Title = task.Title;
            objFromDb.ProjectId = task.ProjectId;
            objFromDb.Project = task.Project;
            objFromDb.UserId = task.UserId;
            objFromDb.User = task.User;
            objFromDb.TaskType = task.TaskType;
            objFromDb.EstimateTime = task.EstimateTime;
            objFromDb.LogedTime = task.LogedTime;
            objFromDb.TaskStatus = task.TaskStatus;
            objFromDb.TaskPriority = task.TaskPriority;
            objFromDb.BeforeVersion = task.BeforeVersion;
            objFromDb.AfterVersion = task.AfterVersion;
            objFromDb.Description = task.Description;
            objFromDb.Reporter = task.Reporter;
            objFromDb.ReporterId = task.ReporterId;
            _db.SaveChanges();
        }
    }
}

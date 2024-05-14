using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
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
            objFromDb.Project = task.Project;
            objFromDb.ProjectId = task.ProjectId;
            objFromDb.Title = task.Title;
            objFromDb.Priority = task.Priority;
            objFromDb.Assignto = task.Assignto;
            objFromDb.AssigntoId = task.AssigntoId;
            objFromDb.ReporterId = task.ReporterId;
            objFromDb.EstimateTime = task.EstimateTime;
            objFromDb.RemainingTime = task.RemainingTime;
            objFromDb.LoggedTime = task.LoggedTime;
            objFromDb.Description = task.Description;
        }
    }
}

using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            objFromDb.TaskStatus = task.TaskStatus;
            objFromDb.TaskType = task.TaskType;
        }

        public IEnumerable<Tasks> GetAllByFilterIncludeRelations(Expression<Func<Tasks, bool>>? filter = null)
        {
            return _db.Tasks.Where(filter).Include(u => u.Project).ToList();
        }
    }
}

using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Repository
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
            objFromDb.Title = project.Title;
            objFromDb.Owner = project.Owner;
            objFromDb.OwnerId = project.OwnerId;
            objFromDb.TasksStatusPercent = project.TasksStatusPercent;
            objFromDb.TasksCount = project.TasksCount;
            if (objFromDb.ImageName == project.ImageName)
                objFromDb.ImageName = project.ImageName;
        }
    }
}

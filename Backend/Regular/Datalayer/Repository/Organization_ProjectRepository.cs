using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class Organization_ProjectRepository : Repository<Organization_Project>, IOrganization_ProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public Organization_ProjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Organization_Project organization_Project)
        {
            var objFromDb = _db.Organization_Project.FirstOrDefault(u => u.Id == organization_Project.Id);
            objFromDb.Organization = organization_Project.Organization;
            objFromDb.OrganizationId = organization_Project.OrganizationId;
            objFromDb.Project = organization_Project.Project;
            objFromDb.ProjectId = organization_Project.ProjectId;
        }
    }
}

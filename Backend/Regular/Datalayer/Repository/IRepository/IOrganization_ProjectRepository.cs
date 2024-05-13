using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface IOrganization_ProjectRepository : IRepository<Organization_Project>
    {
        void Update(Organization_Project organization_Project);
    }
}

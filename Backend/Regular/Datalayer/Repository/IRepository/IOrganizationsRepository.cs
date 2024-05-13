using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface IOrganizationsRepository : IRepository<Organizations>
    {
        void Update(Organizations organizations);
    }
}

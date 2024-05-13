using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface IProjectsRepository : IRepository<Projects>
    {
        void Update(Projects project);
    }
}

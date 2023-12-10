using DataLayer.Models;

namespace DataLayer.Repository.IRepository
{
    public interface IProjectsRepository : IRepository<Projects>
    {
        void Update(Projects project);
    }
}

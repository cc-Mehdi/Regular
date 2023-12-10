using DataLayer.Models;

namespace DataLayer.Repository.IRepository
{
    public interface ITasksRepository : IRepository<Tasks>
    {
        void Update(Tasks task);
    }
}

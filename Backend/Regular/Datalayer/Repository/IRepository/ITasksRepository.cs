using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface ITasksRepository : IRepository<Tasks>
    {
        void Update(Tasks task);
    }
}

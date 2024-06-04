using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface ITasksRepository : IRepository<Tasks>
    {
        IEnumerable<Tasks> GetAllByFilterIncludeRelations(Expression<Func<Tasks, bool>>? filter = null);
        void Update(Tasks task);
    }
}

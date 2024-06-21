using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface IProjectsRepository : IRepository<Projects>
    {
        void Update(Projects project);
        IEnumerable<Projects> GetAllByFilterIncludeRelations(Expression<Func<Projects, bool>>? filter = null);
    }
}

using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface IOrganizationsRepository : IRepository<Organizations>
    {
        IEnumerable<Organizations> GetAllByFilterIncludeRelations(Expression<Func<Organizations, bool>>? filter = null);
        void Update(Organizations organizations);
    }
}

using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface IOrganizations_UsersRepository : IRepository<Organizations_Users>
    {
        void Update(Organizations_Users organizations_Users);
        IEnumerable<Organizations_Users> GetAllByFilterIncludeRelations(Expression<Func<Organizations_Users, bool>>? filter = null);
    }
}

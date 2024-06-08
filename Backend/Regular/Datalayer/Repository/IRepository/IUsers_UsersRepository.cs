using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface IUsers_UsersRepository : IRepository<Users_Users>
    {
        void Update(Users_Users users_Users);
        IEnumerable<Users_Users> GetAllByFilterIncludeRelations(Expression<Func<Users_Users, bool>>? filter = null);
    }
}

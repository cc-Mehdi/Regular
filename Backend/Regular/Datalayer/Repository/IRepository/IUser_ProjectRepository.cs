using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface IUser_ProjectRepository : IRepository<User_Project>
    {
        IEnumerable<User_Project> GetAllByFilterIncludeRelations(Expression<Func<User_Project, bool>>? filter = null);
        void Update(User_Project user_Project);
    }
}

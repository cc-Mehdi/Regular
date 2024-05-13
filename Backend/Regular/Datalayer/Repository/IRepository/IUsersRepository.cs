using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface IUsersRepository : IRepository<Users>
    {
        void Update(Users user);
    }
}

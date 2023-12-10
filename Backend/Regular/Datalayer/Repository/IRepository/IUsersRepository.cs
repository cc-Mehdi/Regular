using DataLayer.Models;

namespace DataLayer.Repository.IRepository
{
    public interface IUsersRepository : IRepository<Users>
    {
        void Update(Users user);
    }
}

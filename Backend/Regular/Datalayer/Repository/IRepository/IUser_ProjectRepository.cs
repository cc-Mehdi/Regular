using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface IUser_ProjectRepository : IRepository<User_Project>
    {
        void Update(User_Project user_Project);
    }
}

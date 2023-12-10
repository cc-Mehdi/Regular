using DataLayer.Models;

namespace DataLayer.Repository.IRepository
{
    public interface IFriendsRepository : IRepository<Friends>
    {
        void Update(Friends friend);
    }
}

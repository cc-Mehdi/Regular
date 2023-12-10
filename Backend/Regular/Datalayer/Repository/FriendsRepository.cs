using DataLayer.Data;
using DataLayer.Models;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
{
    public class FriendsRepository : Repository<Friends>, IFriendsRepository
    {
        private readonly ApplicationDbContext _db;
        public FriendsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Friends friend)
        {
            var objFromDb = _db.Friends.FirstOrDefault(u => u.Id == friend.Id);
            objFromDb.UserId1 = friend.UserId1;
            objFromDb.UserId2 = friend.UserId2;
        }
    }
}

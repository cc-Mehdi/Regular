using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datalayer.Repository
{
    public class Users_UsersRepository : Repository<Users_Users>, IUsers_UsersRepository
    {
        private readonly ApplicationDbContext _db;
        public Users_UsersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Users_Users> GetAllByFilterIncludeRelations(Expression<Func<Users_Users, bool>>? filter = null)
        {
            return _db.Users_Users.Where(filter).Include(u => u.SenderUser).Include(u=> u.ReceiverUser).ToList();
        }

        public void Update(Users_Users users_Users)
        {
            var objFromDb = _db.Users_Users.FirstOrDefault(u => u.Id == users_Users.Id);
            objFromDb.SenderUser = users_Users.SenderUser;
            objFromDb.SenderUserId = users_Users.SenderUserId ;
            objFromDb.ReceiverUser = users_Users.ReceiverUser ;
            objFromDb.ReceiverUserId = users_Users.ReceiverUserId ;
            objFromDb.CreateInviteDate = users_Users.CreateInviteDate ;
            objFromDb.AcceptInviteDate = users_Users.AcceptInviteDate;
        }
    }
}

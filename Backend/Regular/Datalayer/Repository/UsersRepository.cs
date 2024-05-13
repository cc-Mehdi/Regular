using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        public UsersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Users user)
        {
            var objFromDb = _db.Users.FirstOrDefault(u => u.Id == user.Id);
            objFromDb.FullName = user.FullName;
            objFromDb.Username = user.Username;
            objFromDb.Password = user.Password;
            objFromDb.Email = user.Email;
            if (user.ImageName != "")
                objFromDb.ImageName = user.ImageName;
        }
    }
}

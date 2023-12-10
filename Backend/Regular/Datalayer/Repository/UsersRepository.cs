using DataLayer.Data;
using DataLayer.Models;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
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
            objFromDb.Email = user.Email;
            objFromDb.Password = user.Password;
            objFromDb.Username = user.Username;
            objFromDb.FirstName = user.FirstName;
            objFromDb.LastName = user.LastName;
        }
    }
}

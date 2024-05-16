using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class LoginsLogRepository : Repository<LoginsLog>, ILoginsLogRepository
    {
        private readonly ApplicationDbContext _db;
        public LoginsLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LoginsLog loginsLog)
        {
            var objFromDb = _db.LoginsLog.FirstOrDefault(u => u.Id == loginsLog.Id);
            objFromDb.User = loginsLog.User;
            objFromDb.UserId = loginsLog.UserId;
            objFromDb.LoginToken = loginsLog.LoginToken;
            objFromDb.IsSignOut = loginsLog.IsSignOut;
            objFromDb.LogTime = loginsLog.LogTime;
        }
    }
}

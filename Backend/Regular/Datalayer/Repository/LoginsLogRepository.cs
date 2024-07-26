using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Datalayer.Repository
{
    public class LoginsLogRepository : Repository<LoginsLog>, ILoginsLogRepository
    {
        private readonly ApplicationDbContext _db;
        public LoginsLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<LoginsLog> GetAllByFilterIncludeRelations(Expression<Func<LoginsLog, bool>>? filter = null)
        {
            IQueryable<LoginsLog> query = _db.LoginsLog.Include(u => u.User);
            if (filter != null)
                query = query.Where(filter);
            return query.ToList();
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

using Datalayer.Models;
using System.Linq.Expressions;

namespace Datalayer.Repository.IRepository
{
    public interface ILoginsLogRepository : IRepository<LoginsLog>
    {
        void Update(LoginsLog loginsLog);
        IEnumerable<LoginsLog> GetAllByFilterIncludeRelations(Expression<Func<LoginsLog, bool>>? filter = null);

    }
}

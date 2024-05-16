using Datalayer.Models;

namespace Datalayer.Repository.IRepository
{
    public interface ILoginsLogRepository : IRepository<LoginsLog>
    {
        void Update(LoginsLog loginsLog);
    }
}

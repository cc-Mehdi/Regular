using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Repository.IRepository
{
    public interface IEmployeeInvitesRepository : IRepository<EmployeeInvites>
    {
        IEnumerable<EmployeeInvites> GetAllByFilterIncludeUsers(Expression<Func<EmployeeInvites, bool>>? filter = null);
        void Update(EmployeeInvites employeeInvite);

    }
}

using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Repository.IRepository
{
    public interface IEmployeeInvitesRepository : IRepository<EmployeeInvites>
    {
        void Update(EmployeeInvites employeeInvite);
    }
}

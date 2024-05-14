using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;

namespace Datalayer.Repository
{
    public class EmployeeInvitesRepository : Repository<EmployeeInvites>, IEmployeeInvitesRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeInvitesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(EmployeeInvites employeeInvites)
        {
            var objFromDb = _db.EmployeeInvites.FirstOrDefault(u => u.Id == employeeInvites.Id);
            objFromDb.InviteStatus = employeeInvites.InviteStatus;
            objFromDb.Organization = employeeInvites.Organization;
            objFromDb.OrganizationId = employeeInvites.OrganizationId;
            objFromDb.User = employeeInvites.User;
            objFromDb.UserId = employeeInvites.UserId;
        }
    }
}

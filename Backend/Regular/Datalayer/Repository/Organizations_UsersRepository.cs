using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Datalayer.Repository
{
    public class Organizations_UsersRepository : Repository<Organizations_Users>, IOrganizations_UsersRepository
    {
        private readonly ApplicationDbContext _db;
        public Organizations_UsersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Organizations_Users> GetAllByFilterIncludeRelations(Expression<Func<Organizations_Users, bool>>? filter = null)
        {
            IQueryable<Organizations_Users> query = _db.Organizations_Users.Include(u => u.Organization).Include(u => u.User).Include(u=>u.Organization.Owner);
            if (filter != null)
                query = query.Where(filter);
            return query.ToList();
        }

        //public void Update(Users_Users users_Users)
        //{
        //    var objFromDb = _db.Users_Users.FirstOrDefault(u => u.Id == users_Users.Id);
        //    objFromDb.SenderUser = users_Users.SenderUser;
        //    objFromDb.SenderUserId = users_Users.SenderUserId ;
        //    objFromDb.ReceiverUser = users_Users.ReceiverUser ;
        //    objFromDb.ReceiverUserId = users_Users.ReceiverUserId ;
        //    objFromDb.CreateInviteDate = users_Users.CreateInviteDate ;
        //    objFromDb.AcceptInviteDate = users_Users.AcceptInviteDate;
        //}

        public void Update(Organizations_Users organizations_Users)
        {
            var objFromDb = _db.Organizations_Users.FirstOrDefault(u => u.Id == organizations_Users.Id);
            objFromDb.OrganizationId = organizations_Users.OrganizationId;
            objFromDb.Organization = organizations_Users.Organization;
            objFromDb.UserId = organizations_Users.UserId;
            objFromDb.User = organizations_Users.User;
            objFromDb.InviteStatus = organizations_Users.InviteStatus;
            objFromDb.CreateInviteDate = organizations_Users.CreateInviteDate;
            objFromDb.AcceptInviteDate = organizations_Users.AcceptInviteDate;
        }
    }
}

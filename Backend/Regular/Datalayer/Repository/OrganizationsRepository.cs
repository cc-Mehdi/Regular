using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Repository
{
    public class OrganizationsRepository : Repository<Organizations>, IOrganizationsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrganizationsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Organizations> GetAllByFilterIncludeRelations(Expression<Func<Organizations, bool>>? filter = null)
        {
            IQueryable<Organizations> query = _db.Organizations.Include(u => u.Owner);
            if (filter != null)
                query = query.Where(filter);
            return query.ToList();
        }

        public void Update(Organizations organization)
        {
            var objFromDb = _db.Organizations.FirstOrDefault(u => u.Id == organization.Id);
            objFromDb.Title = organization.Title;
            objFromDb.Owner = organization.Owner;
            objFromDb.OwnerId = organization.OwnerId;
            if (objFromDb.ImageName != "")
                objFromDb.ImageName = organization.ImageName;
        }
    }
}

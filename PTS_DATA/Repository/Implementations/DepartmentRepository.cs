using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDBContext _db;
        public DepartmentRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Department entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Departments.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Departments
             .Where(x => x.IsDeleted == false)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetByIdAsync(string id)
        {
            var result = await _db.Departments
          .Where(x => x.Id.ToLower() == id.ToLower())
          .ToListAsync();
            return result ?? null;
        }

        public async Task<Department> GetModelByIdAsync(string id)
        {
            return await _db.Departments
             .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Department>> InactiveDepartments(CancellationToken cancellationToken = default)
        {
            return await _db.Departments
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> SearchDepartments(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Departments
              .Where(x => x.Name.Contains(keyword.Trim()) || x.Name.ToLower() == keyword.Trim().ToLower())
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Department entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}

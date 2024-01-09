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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDBContext _db;
        public EmployeeRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Employee entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Employees.AddAsync(entity);
            if(response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Employees
                .Include(x => x.ApplicationUser)
                .Where(x => x.IsDeleted == false && x.ApplicationUser.IsDeleted == false)
                .ToListAsync(cancellationToken);
        }


        public async Task<IEnumerable<Employee>> GetByEmailAsync(string email)
        {
            var result = await _db.Employees
                .Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUser.Email.ToLower() == email.ToLower())
                .ToListAsync();
            return result ?? null;
        }

        public async Task<IEnumerable<Employee>> GetByIdAsync(string id)
        {
            var result = await _db.Employees
                .Include(x => x.ApplicationUser)
                .Where(x => x.Id.ToLower() == id.ToLower() || x.StaffIdentityCardNumber.ToLower() == id.ToLower())
                .ToListAsync();
            return result ?? null;
        }

        public async Task<Employee> GetModelByIdAsync(string id)
        {
            return await _db.Employees.Include(x => x.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower() || x.StaffIdentityCardNumber.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Employee>> InactiveEmployees(CancellationToken cancellationToken = default)
        {
            return await _db.Employees
               .Include(x => x.ApplicationUser)
               .Where(x => x.IsDeleted == true && x.ApplicationUser.IsDeleted == true)
               .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> SearchEmployees(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Employees
              .Include(x => x.ApplicationUser)
              .Where(x => x.StaffIdentityCardNumber.ToLower() == keyword.ToLower() || 
                     x.ApplicationUser.Email.ToLower() == keyword.ToLower() || 
                     x.ApplicationUser.PhoneNumber.ToLower() == keyword.ToLower())
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}

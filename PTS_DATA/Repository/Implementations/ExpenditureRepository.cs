using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;


namespace PTS_DATA.Repository.Implementations
{
    public class ExpenditureRepository : IExpenditureRepository
    {
        private readonly ApplicationDBContext _db;
        public ExpenditureRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Expenditure entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Expenditures.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expenditure>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
             .Where(x => x.IsDeleted == false)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetAllForAuditor(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
        .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
        x.IsAuditorCommented == false && x.IsDDPCommented == false && x.IsProcurementApproved == true && x.UnitPrice != null)
        .OrderByDescending(x => x.DateCreated)
        .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetAllForChairman(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
       .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
       x.IsAuditorCommented == true && x.IsDDPCommented == true && x.IsProcurementApproved == true && x.UnitPrice != null)
       .OrderByDescending(x => x.DateCreated)
       .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetAllForDDP(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
       .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
       x.IsAuditorCommented == true && x.IsDDPCommented == false && x.IsProcurementApproved == true && x.UnitPrice != null)
       .OrderByDescending(x => x.DateCreated)
       .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetAllForFinance(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
      .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == true && x.IsResolved == false &&
      x.IsAuditorCommented == true && x.IsDDPCommented == true && x.IsProcurementApproved == true && x.UnitPrice != null)
      .OrderByDescending(x => x.DateCreated)
      .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetAllForProcumentOfficer(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
        .Where(x => x.IsDeleted == false&& x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
        x.IsAuditorCommented == false && x.IsDDPCommented == false && x.IsProcurementApproved == false && x.UnitPrice == null)
        .OrderByDescending(x => x.DateCreated)
        .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetByIdAsync(string id)
        {
            return await _db.Expenditures
                            .Where(x => x.Id == id)
                            .ToListAsync();
        }

        public async Task<Expenditure> GetModelByIdAsync(string id)
        {
            return await _db.Expenditures
                       .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Expenditure>> InactiveExpenditure(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> ResolvedRequest(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
            .Where(x => x.IsResolved == true && x.IsVerified == false)
            .OrderByDescending(x => x.LastModified)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> SearchExpenditure(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
          .Where(x => x.CreatorName.Contains(keyword) || x.CreatorName.ToLower() == keyword.ToLower()||
          x.StoreItemName.Contains(keyword) || x.TerminalName.Contains(keyword)||
          x.StoreItemName.ToLower() == keyword.ToLower() || x.TerminalName.ToLower() == keyword.ToLower())
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Expenditure entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}

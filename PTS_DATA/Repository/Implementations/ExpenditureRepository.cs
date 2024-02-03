using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System.Threading;


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
             .Where(x => x.IsDeleted == false && x.IsResolved == true)
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

        public async Task<long> NumberOfFinancialrequestForAuditor()
        {
            return await _db.Expenditures
      .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
      x.IsAuditorCommented == false && x.IsDDPCommented == false && x.IsProcurementApproved == true && x.UnitPrice != null).CountAsync();
        }

        public async Task<long> NumberOfFinancialrequestForChairman()
        {
            return await _db.Expenditures
       .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
       x.IsAuditorCommented == true && x.IsDDPCommented == true && x.IsProcurementApproved == true && x.UnitPrice != null).CountAsync();
        }

        public async Task<long> NumberOfFinancialrequestForDDP()
        {
            return await _db.Expenditures
      .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
      x.IsAuditorCommented == true && x.IsDDPCommented == false && x.IsProcurementApproved == true && x.UnitPrice != null).CountAsync();
        }

        public async Task<long> NumberOfFinancialrequestForFinance()
        {
            return await _db.Expenditures
    .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == true && x.IsResolved == false &&
    x.IsAuditorCommented == true && x.IsDDPCommented == true && x.IsProcurementApproved == true && x.UnitPrice != null).CountAsync();
        }

        public async Task<long> NumberOfFinancialrequestForProcurementOfficer()
        {
            return await _db.Expenditures
        .Where(x => x.IsDeleted == false && x.IsVerified == false && x.IsChairmanApproved == false && x.IsResolved == false &&
        x.IsAuditorCommented == false && x.IsDDPCommented == false && x.IsProcurementApproved == false && x.UnitPrice == null).CountAsync();
        }

        public async Task<long> NumberOfMyRequest(string mail)
        {
            return await _db.Expenditures
          .Where(x => (x.CreatorName.Contains(mail.Trim()) || x.CreatorName.ToLower() == mail.Trim().ToLower()) && x.IsResolved == true).CountAsync();
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

        public async Task<decimal> ThisYearExpenditure(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
                .Where(x => x.IsDeleted == false && x.DateCreated.Value.Date.Year == DateTime.Now.Date.Year).SumAsync(x => x.ItemQuantity.Value*x.UnitPrice.Value);
        }

        public async Task<decimal> TodayExpenditure(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
                .Where(x => x.IsDeleted == false && x.DateCreated.Value.Date == DateTime.Now.Date && x.IsChairmanApproved == true && x.IsAuditorCommented == true && x.IsDDPCommented == true).SumAsync(x => x.ItemQuantity.Value * x.UnitPrice.Value);
        }

        public async Task UpdateAsync(Expenditure entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}

﻿using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IExpenditureRepository : IBaseRepository<Expenditure>
    {
        Task<Expenditure> GetModelByIdAsync(string id);
        Task<IEnumerable<Expenditure>> InactiveExpenditure(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> ResolvedRequest(CancellationToken cancellationToken = default);
        Task<decimal> ThisYearExpenditure(CancellationToken cancellationToken = default);
        Task<decimal> TodayExpenditure(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> GetAllForProcumentOfficer(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> GetAllForAuditor(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> GetAllForDDP(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> GetAllForChairman(CancellationToken cancellationToken = default);
        Task<long> NumberOfMyRequest(string mail);
        Task<long> NumberOfFinancialrequestForFinance();
        Task<long> NumberOfFinancialrequestForChairman();
        Task<long> NumberOfFinancialrequestForAuditor();
        Task<long> NumberOfFinancialrequestForDDP();
        Task<long> NumberOfFinancialrequestForProcurementOfficer();
        Task<IEnumerable<Expenditure>> GetAllForFinance(CancellationToken cancellationToken = default);
        Task<IEnumerable<Expenditure>> SearchExpenditure(string? keyword,CancellationToken cancellationToken = default);
    }
}

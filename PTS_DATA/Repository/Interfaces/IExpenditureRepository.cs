using PTS_CORE.Domain.Entities;
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
    }
}

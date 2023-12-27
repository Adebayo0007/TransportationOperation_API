using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Implementations
{
    public class TerminalRepository : ITerminalRepository
    {
        public Task<bool> CreateAsync(Terminal entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Terminal entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Terminal>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Terminal>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Terminal entity)
        {
            throw new NotImplementedException();
        }
    }
}

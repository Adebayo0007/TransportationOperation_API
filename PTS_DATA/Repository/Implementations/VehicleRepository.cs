using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        public Task<bool> CreateAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }
    }
}

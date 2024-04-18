using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEVStationRepository
    {
        Task<IEnumerable<EVStation>> GetAllAsync();
        Task<EVStation> GetByIdAsync(int id);
        Task AddAsync(EVStation evStation);
    }
}

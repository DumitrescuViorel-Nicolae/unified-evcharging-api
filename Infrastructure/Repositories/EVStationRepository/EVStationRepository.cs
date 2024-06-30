using Domain.Entities;
using Domain.Interfaces.EVStationRepository;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.EVStationRepository
{
    public class EVStationRepository : Repository<EVStation>, IEVStationRepository
    {
        public EVStationRepository(ApplicationDBContext context) : base(context) { }

        public async Task<IEnumerable<EVStation>> GetStationsPerCompany(string companyName)
        {
            var result = await _dbSet.ToListAsync();
            var stations = result.Where(station => station.CompanyName == companyName);
            return stations;
        }
    }

    
}

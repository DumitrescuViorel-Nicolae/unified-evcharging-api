using Domain.Entities;
using Domain.Interfaces.EVStationRepository;
using Infrastructure.Data;
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
    }
}

using Domain.Entities;
using Domain.Interfaces.EVStationRepository;
using Infrastructure.Data;

namespace Infrastructure.Repositories.EVStationRepository
{
    public class EVStationRepository : Repository<EVStation>, IEVStationRepository
    {
        public EVStationRepository(ApplicationDBContext context) : base(context) { }
    }
}

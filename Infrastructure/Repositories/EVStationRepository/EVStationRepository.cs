using Domain.Entities;
using Domain.Interfaces.EVStationRepository;
using Infrastructure.Data;

namespace Infrastructure.Repositories.EVStationRepository
{
    public class EVStationRepository : Repository<EVStation>, IEVStationRepository
    {
        public EVStationRepository(ApplicationDBContext context) : base(context) { }

        public async Task LinkStripeAccountID(int evStationId, string stripeAccountId)
        {
            var evStation = await _dbSet.FindAsync(evStationId);
            if (evStation == null)
                throw new Exception("EVStation not found.");

            evStation.StripeAccountID = stripeAccountId;
            await _context.SaveChangesAsync();
        }
    }
}

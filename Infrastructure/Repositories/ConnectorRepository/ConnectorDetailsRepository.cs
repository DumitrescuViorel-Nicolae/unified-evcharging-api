using Domain.Entities;
using Domain.Interfaces.ConnectorRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ConnectorRepository
{
    public class ConnectorDetailsRepository : Repository<ConnectorDetail>, IConnectorDetailsRepository
    {
        public ConnectorDetailsRepository(ApplicationDBContext context) : base(context) { }
        public async Task DeleteByEvStationIDAsync(int evStationID)
        {
            var detail = await _dbSet.FirstOrDefaultAsync(c => c.EvStationId == evStationID);
            if (detail == null)
                throw new Exception("EVStation not found.");

            _dbSet.Remove(detail);
            await _context.SaveChangesAsync();
        }
    }

}

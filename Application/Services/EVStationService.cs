using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.EVStationRepository;

namespace Application.Services
{
    public class EVStationService : IEVStationService
    {
        private readonly IEVStationRepository _evStations;
        public EVStationService(IEVStationRepository evStations)
        {
            _evStations= evStations;
        }

        public async Task<IEnumerable<EVStation>> GetAll()
        {
            return await _evStations.GetAllAsync();
        }
    }
}

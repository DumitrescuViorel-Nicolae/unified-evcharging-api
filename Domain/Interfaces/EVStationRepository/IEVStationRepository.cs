using Domain.Entities;

namespace Domain.Interfaces.EVStationRepository
{
    public interface IEVStationRepository : IRepository<EVStation>
    {
        Task<IEnumerable<EVStation>> GetStationsPerCompany(string companyName);
    }
}

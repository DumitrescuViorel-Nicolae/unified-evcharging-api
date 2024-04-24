using Domain.Entities;

namespace Domain.Interfaces.EVStationRepository
{
    public interface IEVStationRepository : IRepository<EVStation>
    {
        Task LinkStripeAccountID(int evStationId, string stripeAccountId);
    }
}

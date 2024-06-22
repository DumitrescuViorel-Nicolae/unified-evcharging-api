using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.ConnectorRepository
{
    public interface IConnectorDetailsRepository : IRepository<ConnectorDetail>
    {
        Task DeleteByEvStationIDAsync(int evStationID);
    }
}

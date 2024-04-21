using Domain.Entities;
using Domain.Interfaces.ConnectorRepository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ConnectorRepository
{
    public class ConnectorStatusRepository : Repository<ConnectorStatus>, IConnectorStatusRepository
    {
        public ConnectorStatusRepository(ApplicationDBContext context) : base(context) { }
    }
}

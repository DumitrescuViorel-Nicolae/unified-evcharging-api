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
    public class ConnectorDetailsRepository : Repository<ConnectorDetail>, IConnectorDetailsRepository
    {
        public ConnectorDetailsRepository(ApplicationDBContext context) : base(context) { }
    }
}

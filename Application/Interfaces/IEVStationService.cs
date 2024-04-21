using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEVStationService
    {
        Task<IEnumerable<EVStation>> GetAll();
        Task<IEnumerable<EVStationDTO>> GetEVStations();
        Task<IEnumerable<ConnectorDetail>> GetAllConnectors();
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethods();
    }
}

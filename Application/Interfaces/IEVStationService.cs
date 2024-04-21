using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEVStationService
    {
        Task<List<EVStationDTO>> GetEVStations();
        Task<GeneralResponse<string>> AddEVStation(EVStation newEVStation);
        Task<ConnectorDetail> GetConnectorDetails(int evStationID);
        Task<PaymentMethod> GetPaymentMethods(int evStationID);
        Task<GeneralResponse<string>> DeleteEVStationById(int id);
    }
}

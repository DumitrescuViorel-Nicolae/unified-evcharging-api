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
        Task<List<EVStationDTO>> GetEVStations(Location location);
        Task<GeneralResponse<string>> AddEVStation(AddEVStationDTO newEVStation);
        // Task<ConnectorDetail> GetConnectorDetails(int evStationID);
        Task<List<ConnectorType>> GetConnectorType();
        Task<PaymentMethod> GetPaymentMethods(int evStationID);
        Task<RegisteredCompany> GetRegisteredCompany(int id);
        Task<GeneralResponse<string>> DeleteEVStationById(int id);
    }
}

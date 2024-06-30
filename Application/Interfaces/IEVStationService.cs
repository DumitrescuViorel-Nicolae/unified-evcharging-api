using Domain.DTOs;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IEVStationService
    {
        Task<List<EVStationDTO>> GetEVStations(Location location);
        Task<GeneralResponse<string>> AddEVStation(AddEVStationDTO newEVStation);
        Task<List<ConnectorType>> GetConnectorType();
        Task<PaymentMethod> GetPaymentMethods(int evStationID);
        Task<RegisteredCompany> GetCompanyByUserIdAsync(string userID);
        Task<GeneralResponse<string>> DeleteEVStationById(int id);
        Task<List<EVStationDTO>> GetEVStationsPerCompany(string companyName);
    }
}

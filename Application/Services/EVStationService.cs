using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.ConnectorRepository;
using Domain.Interfaces.EVStationRepository;
using Domain.Interfaces.PaymentRepository;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Domain.Models;
using Infrastructure.Repositories.CompaniesRepository;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace Application.Services
{
    public class EVStationService : IEVStationService
    {
        private readonly IEVStationRepository _evStations;
        private readonly IConnectorDetailsRepository _connectorDetails;
        private readonly IConnectorStatusRepository _connectorStatusRepository;
        private readonly IPaymentMethodsRepository _paymentMethods;
        private readonly ICompaniesRepository _companies;
        private readonly IMapper _mapper;
        private readonly string googleApiKey;

        public EVStationService(IEVStationRepository evStations, IConnectorDetailsRepository connectorDetails,
            IPaymentMethodsRepository paymentMethods, IConfiguration configuration,
            IConnectorStatusRepository connectorStatusRepository, IPaymentService paymentService, IMapper mapper, ICompaniesRepository companies)
        {
            _evStations = evStations;
            _connectorDetails = connectorDetails;
            _paymentMethods = paymentMethods;
            _connectorStatusRepository = connectorStatusRepository;
            _companies = companies;
            googleApiKey = configuration["Google:ApiKey"];
            _mapper = mapper;

        }

        public async Task<List<ConnectorType>> GetConnectorType()
        {
            var connectorDetailsFromDB = await _connectorDetails.GetAllAsync();
            var types = connectorDetailsFromDB.Select(item => new ConnectorType { Id = item.Id, Description = item.ConnectorType });
            return types.ToList();
        }

        public async Task<PaymentMethod> GetPaymentMethods(int evStationID)
        {
            return await _paymentMethods.GetByIdAsync(evStationID);
        }
        public async Task<List<EVStationDTO>> GetEVStations(Location location)
        {
            var evStations = await _evStations.GetAllAsync(station => station.ConnectorDetail,
                                                            station => station.PaymentMethod,
                                                            station => station.Company);

            var connectorStatuses = await _connectorStatusRepository.GetAllAsync();

            var evStationDTOs = evStations.Select(station =>
            {
                var dto = _mapper.Map<EVStationDTO>(station);
                dto.Distance = Math.Round(LocationUtils.CalculateDistance(location, new Location
                {
                    Latitude = station.Latitude,
                    Longitude = station.Longitude
                }), 2);

                // Map connector statuses for each ConnectorDetailDto
                foreach (var connectorDetail in dto.ConnectorDetails)
                {
                    connectorDetail.ConnectorsStatuses = connectorStatuses
                        .Where(status => status.ConnectorDetailsId == connectorDetail.ID)
                        .Select(status => new ConnectorStatusDto
                        {
                            State = status.State,
                            PhysicalReference = status.PhysicalReference
                        }).ToList();
                }

                return dto;
            }).ToList();

            return evStationDTOs;
        }
        public async Task<GeneralResponse<string>> AddEVStation(AddEVStationDTO newEVStation)
        {
            try
            {
                var company = await _companies.GetByNameAsync(newEVStation.CompanyName);
                var evStation = _mapper.Map<EVStation>(newEVStation);
                evStation.CompanyId = company.Id;
                evStation.Website = company.Website;
                evStation.TotalNumberOfConnectors = newEVStation.ConnectorDetails.Count();

                // get coordinates
                var coords = LocationUtils.GetLocationBasedOnAddress(googleApiKey, newEVStation.City, newEVStation.Country, newEVStation.Street);
                evStation.Latitude = coords.Latitude;
                evStation.Longitude = coords.Longitude;

                var addedEVStation = await _evStations.AddAsync(evStation);

                if (addedEVStation != null)
                {
                    var evStationID = addedEVStation.Id;
                    var paymentMethods = _mapper.Map<PaymentMethod>(newEVStation.PaymentMethods);

                    var insertedPaymentMethod = await _paymentMethods.AddAsync(paymentMethods);
                    insertedPaymentMethod.EvStationId = evStationID;

                    foreach (var connectorDetail in newEVStation.ConnectorDetails)
                    {
                        var createdConnectorDetail = _mapper.Map<ConnectorDetail>(connectorDetail);
                        createdConnectorDetail.EvStationId = evStationID;
                        var insertedDetail = await _connectorDetails.AddAsync(createdConnectorDetail);

                        for (int i = 0; i <= connectorDetail.NumberOfConnectors; i++)
                        {
                            var id = insertedDetail.Id;
                            var connectorStatus = new ConnectorStatus
                            {
                                PhysicalReference = i.ToString(),
                                State = "AVAILABLE",
                                ConnectorDetailsId = insertedDetail.Id
                            };
                            await _connectorStatusRepository.AddAsync(connectorStatus);
                        }

                    }

                }

                return new GeneralResponse<string>(true, "New EV Station added successfully!");
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>(false, $"Error in creating a new EV Station - {ex.Message}");
            }
        }
        public async Task<GeneralResponse<string>> DeleteEVStationById(int id)
        {
            try
            {
                var evStationToDelete = await _evStations.GetByIdAsync(id);
                if (evStationToDelete != null)
                {
                    await _paymentMethods.DeleteByEvStationIDAsync(id);
                    await _connectorDetails.DeleteByEvStationIDAsync(id);
                    await _evStations.DeleteAsync(evStationToDelete);

                    return new GeneralResponse<string>(true, "EV Station deleted successfully!");
                }
                else
                {
                    return new GeneralResponse<string>(false, "EV Station not found.");
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>(false, $"Error deleting EV Station: {ex.Message}");
            }
        }
        public async Task<RegisteredCompany> GetCompanyByUserIdAsync(string userID)
        {
            return await _companies.GetByUserIdAsync(userID);
        }
    }
}

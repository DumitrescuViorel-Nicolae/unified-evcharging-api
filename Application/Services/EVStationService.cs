using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.ConnectorRepository;
using Domain.Interfaces.EVStationRepository;
using Domain.Interfaces.PaymentRepository;

namespace Application.Services
{
    public class EVStationService : IEVStationService
    {
        private readonly IEVStationRepository _evStations;
        private readonly IConnectorDetailsRepository _connectorDetails;
        private readonly IConnectorStatusRepository _connectorStatusRepository;
        private readonly IPaymentMethodsRepository _paymentMethods;
        public EVStationService(IEVStationRepository evStations, IConnectorDetailsRepository connectorDetails, 
            IPaymentMethodsRepository paymentMethods, 
            IConnectorStatusRepository connectorStatusRepository)
        {
            _evStations = evStations;
            _connectorDetails = connectorDetails;
            _paymentMethods = paymentMethods;
            _connectorStatusRepository = connectorStatusRepository;
        }

        public async Task<IEnumerable<EVStation>> GetAll()
        {
            return await _evStations.GetAllAsync();
        }

        public async Task<IEnumerable<ConnectorDetail>> GetAllConnectors()
        {
            return await _connectorDetails.GetAllAsync();
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethods()
        {
            return await _paymentMethods.GetAllAsync();
        }

        public async Task<IEnumerable<EVStationDTO>> GetEVStations()
        {
            var evStations = await _evStations.GetAllAsync(station => station.ConnectorDetail, 
                                                            station => station.PaymentMethod);

            var connectorStatuses = await _connectorStatusRepository.GetAllAsync();

            // to convert into a mapping
            return evStations.Select(station => new EVStationDTO
            {
                Brand = station.Brand,
                TotalNumberOfConnectors = station.ConnectorDetail.Count,
                Address = new Address
                {
                    Street = station.Street,
                    City = station.City,
                    Country = station.Country
                },
                Contacts = new Contacts
                {
                    Phone = station.Phone,
                    Website = station.Website
                },
                Position = new Position
                {
                    Latitude = station.Latitude,
                    Longitude = station.Longitude
                },

                ConnectorDetails = station.ConnectorDetail.Select(detail => new ConnectorDetailDto
                {
                    SupplierName = detail.SupplierName,
                    ConnectorType = detail.ConnectorType,
                    ChargeCapacity = detail.ChargeCapacity,
                    MaxPowerLevel = detail.MaxPowerLevel,
                    CustomerChargeLevel = detail.CustomerChargeLevel,
                    CustomerConnectorName = detail.CustomerConnectorName,
                    ConnectorsStatuses = connectorStatuses.Where(status => status.ConnectorDetailsId == detail.Id).Select(status => new ConnectorStatusDto { 
                        State = status.State,
                        PhysicalReference = status.PhysicalReference
                    }).ToList(),
                }).ToList(),

                PaymentMethods = new PaymentMethodDTO
                {
                    EPayment = new PaymentType
                    {
                        Accept = station.PaymentMethod.EPaymentAccept,
                        Types = new PaymentTypes
                        {
                            Type = station.PaymentMethod.EPaymentTypes.Split(',').ToList(),
                        }
                    },
                    Other = new PaymentType
                    {
                        Accept = station.PaymentMethod.OtherPaymentAccept.GetValueOrDefault(),
                        Types = new PaymentTypes
                        {
                            Type = station.PaymentMethod.OtherPaymentTypes?.Split(',').ToList() ?? new List<string>()
                        }
                    }
                }
            }).ToList();      
        }
    }
}

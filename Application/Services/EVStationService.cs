using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.ConnectorRepository;
using Domain.Interfaces.EVStationRepository;
using Domain.Interfaces.PaymentRepository;
using Domain.Models;

namespace Application.Services
{
    public class EVStationService : IEVStationService
    {
        private readonly IEVStationRepository _evStations;
        private readonly IConnectorDetailsRepository _connectorDetails;
        private readonly IConnectorStatusRepository _connectorStatusRepository;
        private readonly IPaymentMethodsRepository _paymentMethods;
        private readonly IMapper _mapper;


        public EVStationService(IEVStationRepository evStations, IConnectorDetailsRepository connectorDetails,
            IPaymentMethodsRepository paymentMethods,
            IConnectorStatusRepository connectorStatusRepository, IPaymentService paymentService, IMapper mapper)
        {
            _evStations = evStations;
            _connectorDetails = connectorDetails;
            _paymentMethods = paymentMethods;
            _connectorStatusRepository = connectorStatusRepository;
            _mapper = mapper;
        }

        public async Task<ConnectorDetail> GetConnectorDetails(int evStationID)
        {
            return await _connectorDetails.GetByIdAsync(evStationID);
        }

        public async Task<PaymentMethod> GetPaymentMethods(int evStationID)
        {
            return await _paymentMethods.GetByIdAsync(evStationID);
        }

        public async Task<List<EVStationDTO>> GetEVStations()
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
        public async Task<GeneralResponse<string>> AddEVStation(EVStationDTO newEVStation)
        {
            try
            {
                var newStation = _mapper.Map<EVStation>(newEVStation);
                var addedEVStation = await _evStations.AddAsync(newStation);

                if(addedEVStation!= null)
                {
                    var paymentMethods = new PaymentMethod
                    {
                        EvStationId = addedEVStation.Id,
                        EPaymentAccept = addedEVStation.PaymentMethod.EPaymentAccept,
                        OtherPaymentAccept = addedEVStation.PaymentMethod.OtherPaymentAccept,
                        EPaymentTypes = addedEVStation.PaymentMethod.EPaymentTypes,
                        OtherPaymentTypes = addedEVStation.PaymentMethod.OtherPaymentTypes
                    };

                    await _paymentMethods.AddAsync(paymentMethods);

                   foreach(var connectorDetail in addedEVStation.ConnectorDetail)
                    {
                        await _connectorDetails.AddAsync(connectorDetail);
                    }
                }

                return new GeneralResponse<string>(true, "New EV Station added successfully!");
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>(false, $"Error in creating a new EV Station - {ex.Message}");
            }
        }
        public async Task<GeneralResponse<string>> LinkStripeAccountID(int evStationId, string stripeAccountID)
        {
            try
            {
                await _evStations.LinkStripeAccountID(evStationId, stripeAccountID);
                return new GeneralResponse<string>(true, "The selected EV station has been linked");
            }
            catch (Exception e)
            {
                return new GeneralResponse<string>(false, $"Link not successfull - {e.Message}");
            }
        }
        public async Task<GeneralResponse<string>> DeleteEVStationById(int id)
        {
            try
            {
                var evStationToDelete = await _evStations.GetByIdAsync(id);
                if (evStationToDelete != null)
                {
                    await _paymentMethods.DeleteByIdAsync(id);
                    await _connectorDetails.DeleteByIdAsync(id);
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

    }
}

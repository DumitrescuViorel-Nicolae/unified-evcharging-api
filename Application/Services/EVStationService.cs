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
        private readonly IPaymentMethodsRepository _paymentMethods;
        public EVStationService(IEVStationRepository evStations, IConnectorDetailsRepository connectorDetails, IPaymentMethodsRepository paymentMethods)
        {
            _evStations = evStations;
            _connectorDetails = connectorDetails;
            _paymentMethods = paymentMethods;
        }

        public async Task<IEnumerable<EVStation>> GetAll()
        {
            return await _evStations.GetAllAsync();
        }

        public async Task<IEnumerable<EVStation>> GetEVStations()
        {
            var evStations = await _evStations.GetAllAsync(station => station.ConnectorDetail, station => station.PaymentMethod);
            return evStations;
        }

        public async Task<IEnumerable<ConnectorDetail>> GetAllConnectors()
        {
            return await _connectorDetails.GetAllAsync();
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethods()
        {
            return await _paymentMethods.GetAllAsync();
        }
    }
}

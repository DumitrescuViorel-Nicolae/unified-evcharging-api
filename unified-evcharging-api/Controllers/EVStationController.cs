using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EVStationController : ControllerBase
    {
        private readonly IEVStationService _evStationService;
        public EVStationController(IEVStationService evStationService)
        {
            _evStationService = evStationService;
        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<EVStation>> GetAll()
        {
            return await _evStationService.GetAll();
        }

        [HttpGet("getEVInfrastructure")]
        public async Task<IEnumerable<EVStation>> GetInfra()
        {
           var result = await _evStationService.GetEVStations();
            return result;
        }

        [HttpGet("getEVConnectorDetails")]
        public async Task<IEnumerable<ConnectorDetail>> GetConnectors()
        {
            var connectorDetails = await _evStationService.GetAllConnectors();
            return connectorDetails;
        }

        [HttpGet("getPaymentMethods")]
        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
        {
            return await _evStationService.GetAllPaymentMethods();
        }
    }
}

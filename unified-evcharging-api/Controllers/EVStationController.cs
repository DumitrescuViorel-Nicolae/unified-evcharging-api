using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("getEVInfrastructure")]
        public async Task<List<EVStationDTO>> GetEVInfra()
        {
           var result = await _evStationService.GetEVStations();
            return result;
        }

        [HttpPost("addEVStation")]
        
        public async Task<GeneralResponse<string>> AddEVStation(EVStationDTO evStation)
        {
            var response = await _evStationService.AddEVStation(evStation);
            return response;
        }

        [HttpPatch("linkEVStation")]
        [Authorize(Roles = "Company")]
        public async Task<GeneralResponse<string>> LinkEVStationToStripe(int evStationID, string stripeAccountID)
        {
            var response = await _evStationService.LinkStripeAccountID(evStationID, stripeAccountID);
            return response;
        }

        [HttpDelete("deleteEVStation")]
        public async Task<GeneralResponse<string>> DeleteEVStation(int evStationID)
        {
            var response = await _evStationService.DeleteEVStationById(evStationID);
            return response;
        }

        [HttpGet("getEVConnectorDetails")]
        public async Task<ConnectorDetail> GetEVConnectorDetails(int evStationID)
        {
            var connectorDetails = await _evStationService.GetConnectorDetails(evStationID);
            return connectorDetails;
        }

        [HttpGet("getEVPaymentMethods")]
        public async Task<PaymentMethod> GetEVPaymentMethods(int evStationID)
        {
            return await _evStationService.GetPaymentMethods(evStationID);
        }
    }
}

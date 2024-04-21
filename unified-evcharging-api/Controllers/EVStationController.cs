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
    }
}

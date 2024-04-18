using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService= accountService;
        }

        [HttpGet("getAccount")]
        public async Task<IActionResult> GetAccount(string email)
        {
            var response = await _accountService.GetUser(email);

            return Ok(response);
        }
    }
}

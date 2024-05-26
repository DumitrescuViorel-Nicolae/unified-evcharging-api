using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        [HttpPut("updateAccount")]
        public async Task<IActionResult> UpdateAccount(UserDTO user)
        {
            var response = await _accountService.UpdateUser(user);
            return Ok(response);
        }

        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(string accountID)
        {
            var response = await _accountService.DeleteAccount(accountID);
            return Ok(response);
        }
    }
}

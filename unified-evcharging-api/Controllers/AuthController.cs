using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Dependency Injection

        public AuthController()
        {

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<UserDTO> RegisterUser(UserDTO user)
        {
            return Ok(user);
        }
    }
}

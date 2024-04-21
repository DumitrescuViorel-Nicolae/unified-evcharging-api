using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public PaymentController()
        {

        }

        [HttpPost("processEVPayment")]
        public IActionResult ProcessPayment()
        {
            return Ok();
        }
    }
}

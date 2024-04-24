using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("processEVPayment")]
        public async Task<IActionResult> ProcessPayment(string evStationStripeAccountId)
        {
            var payment = await _paymentService.ProcessPayment(evStationStripeAccountId);
            return Ok(payment);
        }

        [HttpPost("createNewStripeEVAccount")]
        public async Task<IActionResult> CreateStripeAccount(StripeEVAccountDetails eVAccountDetails)
        {
            var response = await _paymentService.CreateEVConnectAccount(eVAccountDetails);
            return Ok(response);
        }

        [HttpGet("getStripeEVAccount")]
        public async Task<IActionResult> GetEVAccount(string accountID)
        {
            var response = await _paymentService.GetStripeEVAccount(accountID);
            return Ok(response);
        }

        [HttpDelete("deleteStripeAccount")]
        public async Task<IActionResult> DeleteAccount(string accountID)
        {
            var response = await _paymentService.DeleteAccount(accountID);
            return Ok(response);
        }
    }
}

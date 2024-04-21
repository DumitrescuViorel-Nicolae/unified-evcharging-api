using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private string StripeAPIKey;

        public PaymentService(IConfiguration configuration)
        {
            StripeAPIKey = configuration["Stripe:APIKey"];
        }

        public PaymentIntent ProcessPayment(decimal amount=1, string paymentMethodId = "pm_card_visa")
        {
            StripeConfiguration.ApiKey = StripeAPIKey;
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                PaymentMethod = paymentMethodId,
                Confirm = true
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            if (paymentIntent.Status == "succeeded")
            {
                Console.WriteLine("Payment successful!");
            }
            else
            {
                Console.WriteLine($"Payment failed: {paymentIntent.LastPaymentError?.Message}");
            }

            return paymentIntent;
        }
    }
}

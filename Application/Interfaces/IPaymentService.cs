using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        PaymentIntent ProcessPayment(decimal amount = 1, string paymentMethodId = "pm_card_visa");
    }
}

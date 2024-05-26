using Domain.Entities;
using Domain.Models;
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
        Task<GeneralResponse<PaymentIntent>> ProcessPayment(string evStationStripeAccountId,
            decimal amount = 1,
            string paymentMethodId = "pm_card_visa");
        Task<IEnumerable<PaymentTransaction>> GetTransactions();
        Task<Account> CreateEVConnectAccount(StripeEVAccountDetails stripeEVAccountDetails);
        Task<Account> GetStripeEVAccount(string accountID);
        Task<string> DeleteAccount(string accountID);
    }
}

﻿using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly string StripeAPIKey;
        private readonly Stripe.AccountService _stripeAccountService;
        private readonly PersonService _stripePersonService;
        private readonly PaymentIntentService _paymentIntentService;

        public PaymentService(IConfiguration configuration)
        {
            StripeAPIKey = configuration["Stripe:APIKey"];
            StripeConfiguration.ApiKey = StripeAPIKey;

            _stripeAccountService = new();
            _stripePersonService = new();
            _paymentIntentService = new();
        }

        /// <summary>
        /// Stripe Payment Processor
        /// Initiates a payment intent for the specified EVStation
        /// </summary>
        /// <param name="evStationStripeAccountId"></param>
        /// <param name="amount"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public async Task<GeneralResponse<PaymentIntent>> ProcessPayment(string evStationStripeAccountId,
            decimal amount = 1,
            string paymentMethodId = "pm_card_visa")
        {
            // hardcoded for now
            // stripe test data
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                PaymentMethod = paymentMethodId,
                Confirm = true,
                OnBehalfOf = evStationStripeAccountId,
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = evStationStripeAccountId,
                }
            };

            try
            {
                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                if (paymentIntent.Status == "succeeded")
                {
                    return new GeneralResponse<PaymentIntent>(true, "Payment succeeded", paymentIntent);
                }
                else
                {
                    return new GeneralResponse<PaymentIntent>(false, $"Payment failed - {paymentIntent.LastPaymentError?.Message}");
                }
            }
            catch (Exception e)
            {
                return new GeneralResponse<PaymentIntent>(false, e.Message);
            }
        }

        /// <summary>
        /// Get created Stripe Account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<Account> GetStripeEVAccount(string accountID)
        {
            return await _stripeAccountService.GetAsync(accountID);
        }

        /// <summary>
        /// Create a Stripe Account for EVStation
        /// </summary>
        /// <param name="stripeEVAccountDetails"></param>
        /// <returns></returns>
        public async Task<Account> CreateEVConnectAccount(StripeEVAccountDetails stripeEVAccountDetails)
        {
            // Stripe Account initiation
            string connectedAccountID = await DefineAccountType();
            await DefineBusinessDetails(connectedAccountID, stripeEVAccountDetails);

            // Stripe account verification steps
            // Uses testdata from stripe
            string personID = await AddPersonToBusiness(connectedAccountID);
            await UpdatePersonDetails(connectedAccountID, personID);
            await AddBusinessOwnerForVerification(connectedAccountID);

            return await GetStripeEVAccount(connectedAccountID);
        }

        /// <summary>
        /// Delete an existing Stripe Account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<string> DeleteAccount(string accountID)
        {
            await _stripeAccountService.DeleteAsync(accountID);
            return $"Account has been deleted - {accountID}";
        }


        // Steps for creating a working Stripe Connect Account
        // for each linked EV Station
        private async Task AddBusinessOwnerForVerification(string connectedAccountID)
        {
            var ownerOptions = new PersonCreateOptions
            {
                FirstName = "Kathleen",
                LastName = "Banks",
                Email = "kathleen@bestcookieco.com",
                Relationship = new PersonRelationshipOptions { Owner = true, PercentOwnership = 80M },
                Address = new AddressOptions
                {
                    Line1 = "address_full_match",
                    City = "Olympia",
                    State = "Washington",
                    Country = "US",
                    PostalCode = "98501"
                },
                Phone = "+1 (360) 491-9320",
                SsnLast4 = "0000",
                Dob = new DobOptions
                {
                    Day = 01,
                    Month = 01,
                    Year = 1901
                },
                IdNumber = "000-00-0000"

            };
            await _stripePersonService.CreateAsync(connectedAccountID, ownerOptions);
        }

        private async Task UpdatePersonDetails(string connectedAccountID, string personID)
        {
            var personUpdatesOptions = new PersonUpdateOptions
            {
                Address = new AddressOptions
                {
                    City = "Schenectady",
                    Line1 = "123 State St",
                    PostalCode = "12345",
                    State = "NY",
                },
                Dob = new DobOptions { Day = 10, Month = 11, Year = 1980 },
                SsnLast4 = "0000",
                Phone = "8888675309",
                Email = "jenny@bestcookieco.com",
                Relationship = new PersonRelationshipOptions { Executive = true },
            };

            await _stripePersonService.UpdateAsync(connectedAccountID, personID, personUpdatesOptions);
        }

        private async Task<string> AddPersonToBusiness(string connectedAccountID)
        {
            var personOptions = new PersonCreateOptions
            {
                FirstName = "Jenny",
                LastName = "Rosen",
                Relationship = new PersonRelationshipOptions { Representative = true, Title = "CEO" },
            };

            var person = await _stripePersonService.CreateAsync(connectedAccountID, personOptions);
            var personID = person.Id;
            return personID;
        }

        private async Task<string> DefineAccountType()
        {
            // these should remain unchaged
            // test data provided by Stripe
            var options = new AccountCreateOptions
            {
                Country = "US",
                Type = "custom",
                BusinessType = "company",
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                },
                ExternalAccount = "btok_us",
                TosAcceptance = new AccountTosAcceptanceOptions
                {
                    Date = DateTimeOffset.FromUnixTimeSeconds(1547923073).UtcDateTime,
                    Ip = "172.18.80.19",
                },
            };

            var createdAccount = await _stripeAccountService.CreateAsync(options);
            var connectedAccountID = createdAccount.Id;
            return connectedAccountID;
        }

        private async Task DefineBusinessDetails(string connectedAccountID, StripeEVAccountDetails eVAccountDetails)
        {
            var adressDetails = eVAccountDetails.AdressDetails;
            var updatesOptions = new AccountUpdateOptions
            {
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Mcc = "5045",
                    Url = "https://bestcookieco.com",
                },
                Company = new AccountCompanyOptions
                {
                    Address = new AddressOptions
                    {
                        City = adressDetails.City,
                        Line1 = adressDetails.Line1,
                        PostalCode = adressDetails.PostalCode,
                        State = adressDetails.State,
                    },
                    TaxId = "000000000", //test data
                    Name = eVAccountDetails.EVStationName,
                    Phone = "8888675309", //test data
                },
            };

            await _stripeAccountService.UpdateAsync(connectedAccountID, updatesOptions);
        }
     
    }
}
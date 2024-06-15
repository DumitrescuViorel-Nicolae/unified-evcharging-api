using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class RegisteredCompany
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string StripeAccountID { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string ZipCode { get; set; }

        public ICollection<EVStation> EVStations { get; set; } 
    }
}

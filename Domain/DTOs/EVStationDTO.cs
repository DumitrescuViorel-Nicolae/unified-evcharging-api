using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EVStationDTO
    {
        public string? StripeAccountID { get; set; } 
        public string Brand { get; set; }
        public string CompanyName { get; set; }
        public int TotalNumberOfConnectors { get; set; }
        public Address Address { get; set; }
        public Contacts Contacts { get; set; }
        public Position Position { get; set; }
        public List<ConnectorDetailDto> ConnectorDetails { get; set; }
        public PaymentMethodDTO PaymentMethods { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Contacts
    {
        public string Phone { get; set; }
        public string Website { get; set; }
    }

    public class Position
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StripeEVAccountDetails
    {
        public string EVStationName { get; set; } //same as brand -> infrastructure company provider
        public AdressDetails AdressDetails { get; set; }
    }

    public class AdressDetails
    {
        public string City { get; set; } = "Schenectady";
        public string Line1 { get; set; } = "123 State St";
        public string PostalCode { get; set; } = "12345";
        public string State { get; set; } = "NY";
    }
}

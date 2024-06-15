using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CompanyDTO
    {
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public string? ZipCode { get; set; }
    }
}

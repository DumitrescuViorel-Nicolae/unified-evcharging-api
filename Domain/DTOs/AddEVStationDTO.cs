using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AddEVStationDTO
    {
        public string Brand { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }
        public string? Phone { get; set; }

        public List<ConnectorDetailDto> ConnectorDetails { get; set; }
        public PaymentMethodDTO PaymentMethods { get; set; }

    }
}

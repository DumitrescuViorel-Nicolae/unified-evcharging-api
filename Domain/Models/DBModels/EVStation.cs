using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DBModels
{
    public class EVStation
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public int TotalNumberOfConnectors { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ICollection<ConnectorDetails> ConnectorDetails { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
    }
}

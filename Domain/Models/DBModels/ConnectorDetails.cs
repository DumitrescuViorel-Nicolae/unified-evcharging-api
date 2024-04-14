using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DBModels
{
    public class ConnectorDetails
    {
        public int Id { get; set; }
        public int EvStationId { get; set; }
        public string SupplierName { get; set; }
        public string ConnectorType { get; set; }
        public string ChargeCapacity { get; set; }
        public int MaxPowerLevel { get; set; }
        public string CustomerChargeLevel { get; set; }
        public string CustomerConnectorName { get; set; }
        public bool Pay { get; set; }

        public ICollection<ConnectorStatus> ConnectorStatuses { get; set; }
    }
}

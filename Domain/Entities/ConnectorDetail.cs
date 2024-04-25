using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class ConnectorDetail
    {
        public int Id { get; set; }
        public int? EvStationId { get; set; }
        public string? SupplierName { get; set; }
        public string? ConnectorType { get; set; }
        public string? ChargeCapacity { get; set; }
        public int MaxPowerLevel { get; set; }
        public string? CustomerChargeLevel { get; set; }
        public string? CustomerConnectorName { get; set; }
        public bool? Pay { get; set; }

        [ForeignKey("EVStationId")]
        public EVStation? EVStation { get; set; }
    }
}

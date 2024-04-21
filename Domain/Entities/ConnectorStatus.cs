using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class ConnectorStatus
    {
        public int Id { get; set; }
        public int ConnectorDetailsId { get; set; }
        public string PhysicalReference { get; set; }
        public string? State { get; set; }
    }
}

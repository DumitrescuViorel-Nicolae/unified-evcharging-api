using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DBModels
{
    public class ConnectorStatus
    {
        public int Id { get; set; }
        public int ConnectorDetailId { get; set; }
        public string PhysicalReference { get; set; }
        public string State { get; set; }
    }
}

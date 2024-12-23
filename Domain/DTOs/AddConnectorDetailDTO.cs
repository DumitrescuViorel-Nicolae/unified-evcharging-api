﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AddConnectorDetailDTO
    {
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public string ConnectorType { get; set; }
        public string ChargeCapacity { get; set; }
        public int MaxPowerLevel { get; set; }
        public decimal Price { get; set; }
        public string CustomerChargeLevel { get; set; }
        public string CustomerConnectorName { get; set; }
        public int NumberOfConnectors { get; set; }
    }
}

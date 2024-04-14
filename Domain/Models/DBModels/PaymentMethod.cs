using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DBModels
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public int EvStationId { get; set; }
        public bool EPaymentAccept { get; set; }
        public bool OtherPaymentAccept { get; set; }
        public string EPaymentTypes { get; set; }
        public string OtherPaymentTypes { get; set; }

        public EVStation EVStation { get; set; }
    }
}

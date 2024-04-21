using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class PaymentMethodDTO
    {
        public PaymentType EPayment { get; set; }
        public PaymentType Other { get; set; }
    }

    public class PaymentType
    {
        public bool Accept { get; set; }
        public PaymentTypes Types { get; set; }
    }

    public class PaymentTypes
    {
        public List<string> Type { get; set; }
    }
}

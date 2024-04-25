using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentTransaction
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethodBrand { get; set; }
        public string PaymentMethodLast4 { get; set; }
        public string Status { get; set; }
        public string ReceiptUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

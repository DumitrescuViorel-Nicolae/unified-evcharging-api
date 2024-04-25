using Domain.Entities;
using Domain.Interfaces.PaymentTransactionRepository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.PaymentRepository
{
    public class PaymentTransactions : Repository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactions(ApplicationDBContext context) : base(context)
        {
        }
    }
}

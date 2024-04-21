using Domain.Entities;
using Domain.Interfaces.PaymentRepository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.PaymentRepository
{
    public class PaymentMethodsRepository : Repository<PaymentMethod>, IPaymentMethodsRepository
    {
        public PaymentMethodsRepository(ApplicationDBContext context) : base(context) { }
    }
}

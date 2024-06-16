using Domain.Entities;
using Domain.Interfaces.PaymentRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories.PaymentRepository
{
    public class PaymentMethodsRepository : Repository<PaymentMethod>, IPaymentMethodsRepository
    {
        public PaymentMethodsRepository(ApplicationDBContext context) : base(context) { }

        public async Task DeleteByEvStationIDAsync(int evStationID)
        {
           var paymentMethod = await _dbSet.FirstOrDefaultAsync(c => c.EvStationId == evStationID);
            if (paymentMethod == null)
                throw new Exception("EVStation not found.");
            
            _dbSet.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }
    }
}

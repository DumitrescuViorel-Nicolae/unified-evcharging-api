using Domain.Entities;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.CompaniesRepository
{
    public class CompaniesRepository : Repository<RegisteredCompany>, ICompaniesRepository
    {
        public CompaniesRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task LinkStripeAccountID(int companyId, string stripeAccountId)
        {
            var company = await _dbSet.FindAsync(companyId);
            if (company == null)
                throw new Exception("EVStation not found.");

            company.StripeAccountID = stripeAccountId;
            await _context.SaveChangesAsync();
        }
    }
}

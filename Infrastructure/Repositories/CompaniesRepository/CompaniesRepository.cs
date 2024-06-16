using Domain.Entities;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        public async Task<RegisteredCompany> GetByNameAsync(string companyName)
        {
            var company = await _dbSet.FirstOrDefaultAsync(c => c.CompanyName == companyName);
            if (company == null)
                throw new Exception("EVStation not found.");

            return company;
        }

        public async Task<RegisteredCompany> GetByUserIdAsync(string userID)
        {
            var company = await _dbSet.FirstOrDefaultAsync(c => c.UserId == userID);
            if (company == null)
                throw new Exception("EVStation not found.");

            return company;
        }
    }
}

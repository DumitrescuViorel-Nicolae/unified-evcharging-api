using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.RegisteredCompaniesRepository
{
    public interface ICompaniesRepository : IRepository<RegisteredCompany>
    {
        Task LinkStripeAccountID(int companyId, string stripeAccountId);
    }
}

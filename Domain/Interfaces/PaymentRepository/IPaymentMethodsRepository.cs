﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.PaymentRepository
{
    public interface IPaymentMethodsRepository : IRepository<PaymentMethod>
    {
        Task DeleteByEvStationIDAsync(int evStationID);
    }
}

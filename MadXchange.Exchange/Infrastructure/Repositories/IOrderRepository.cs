using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Infrastructure.Repositories
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        
    }
}

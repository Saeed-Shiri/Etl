using FarasaEtl.Domain.Entities.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Domain.Repositories.Commands
{
    public interface IMarketMakingCommandRepository : ICommandRepository<MarketMakingActivityLicense>
    {
        Task<long> GetLastRequestIdProcessed(CancellationToken cancellationToken = default);
    }
}

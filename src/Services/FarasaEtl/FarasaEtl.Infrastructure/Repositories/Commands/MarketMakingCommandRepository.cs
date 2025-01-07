using FarasaEtl.Domain.Entities.Target;
using FarasaEtl.Domain.Repositories.Commands;
using FarasaEtl.Infrastructure.Data.Target;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Infrastructure.Repositories.Commands
{
    public class MarketMakingCommandRepository : CommandRepository<MarketMakingActivityLicense>, IMarketMakingCommandRepository
    {
        public MarketMakingCommandRepository(MonitoringBaseInfoDbContext monitoringBaseInfoContext) : base(monitoringBaseInfoContext)
        {
        }

        public async Task<long> GetLastRequestIdProcessed(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lastData = await _monitoringBaseInfoContext
                .MarketMakingActivityLicenses
                .OrderBy(x => x.Id)
                .LastOrDefaultAsync();

            return lastData == null ? 0 : lastData.FarasaRequestId;

        }
    }
}

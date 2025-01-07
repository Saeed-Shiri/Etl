

using FarasaEtl.Domain.Entities.Source;

namespace FarasaEtl.Domain.Repositories.Queries
{
    public interface IMarketMakingQueryRepository : IQueryRepository<MarketMakingActivityLicenseSourse>
    {
        Task<IEnumerable<MarketMakingActivityLicenseSourse>> GetMarketMakingActivityLicensesAsync(long lastRequestIdProcessed, CancellationToken cancellationToken = default);
    }
}

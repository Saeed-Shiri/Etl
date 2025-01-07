
using Dapper;
using FarasaEtl.Domain.Entities.Source;
using FarasaEtl.Domain.Repositories.Commands;
using FarasaEtl.Domain.Repositories.Queries;
using System.Data;

namespace FarasaEtl.Infrastructure.Repositories.Queries
{
    public class MarketMakingQueryRepository : QueryRespository<MarketMakingActivityLicenseSourse>, IMarketMakingQueryRepository
    {

        private readonly IMarketMakingCommandRepository _marketMakingCommandRepository;
        public MarketMakingQueryRepository(IDbConnection sourceConnection, IMarketMakingCommandRepository marketMakingCommandRepository) : base(sourceConnection)
        {
            _marketMakingCommandRepository = marketMakingCommandRepository;
        }

        public async Task<IEnumerable<MarketMakingActivityLicenseSourse>> GetMarketMakingActivityLicensesAsync(long lastRequestIdProcessed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // SQL query: Order first, then filter
            var query = @"SELECT *
                FROM (
                    SELECT TOP 100 PERCENT mmal.ID as Id, NationalID, SymbolISIN, MinimumDailyTrade, MinimumStackedOrder, QuotedDomainType, CreatedByRequestInstanceID, ContractTypeId , StartDate ,[EndDate]
  FROM [MarketMakings].[MarketMakingActivityLicense] mmal inner join [service].[Broker] b
  on mmal.BrokerID = b.ID
  order by Id
                ) AS OrderedData
                WHERE Id > (SELECT  ISNULL((SELECT ID FROM [MarketMakings].[MarketMakingActivityLicense] where CreatedByRequestInstanceID = @LastRequestIdProcessed) , 0))
				order by Id";

            // Define parameters
            var parameters = new { LastRequestIdProcessed = lastRequestIdProcessed };

            var result = await _sourceConnection
                .QueryAsync<MarketMakingActivityLicenseSourse>(
                query,
                parameters);

            return result;
        }

    }
}


using FarasaEtl.Application.Enums;
using FarasaEtl.Application.Interfaces;
using FarasaEtl.Application.Settings;
using FarasaEtl.Application.Utilities;
using FarasaEtl.Domain.Entities.Target;
using FarasaEtl.Domain.Repositories.Commands;
using FarasaEtl.Domain.Repositories.Queries;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FarasaEtl.Application.Services.MarketMakingActivity;
public class MarketMakingActivityLicenseService : IRecurringJob
{
    private readonly IMarketMakingQueryRepository _marketMakingQueryRepository;
    private readonly IMarketMakingCommandRepository _marketMakingCommandRepository;
    private readonly ILogger<MarketMakingActivityLicenseService> _logger;
    private readonly IOptionsMonitor<MarketMakingActivityLicenseSetting> _options;


    public MarketMakingActivityLicenseService(
        IMarketMakingQueryRepository marketMakingQueryRepository,
        IMarketMakingCommandRepository marketMakingCommandRepository,
        ILogger<MarketMakingActivityLicenseService> logger,
        IOptionsMonitor<MarketMakingActivityLicenseSetting> options)
    {
        _marketMakingQueryRepository = marketMakingQueryRepository;
        _marketMakingCommandRepository = marketMakingCommandRepository;
        _logger = logger;
        _options = options;
    }

    public string CronExpression => Cron.Minutely();

    public string JobId => _options.CurrentValue.JobId;

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation($"{_options.CurrentValue.JobId} ETL starting...");

        try
        {
            var lastRequestIdProcessed = await _marketMakingCommandRepository
            .GetLastRequestIdProcessed();

            var sourceData = await _marketMakingQueryRepository
                .GetMarketMakingActivityLicensesAsync(lastRequestIdProcessed, cancellationToken);

            _logger.LogInformation($"Extracted {sourceData.Count()} records from source.");

            List<MarketMakingActivityLicense> targetList = new List<MarketMakingActivityLicense>();

            foreach (var item in sourceData)
            {
                _logger.LogInformation(item.ToString());
                targetList.Add(new MarketMakingActivityLicense()
                {
                    MarketMakerNationalId = item.NationalID.ToString(),
                    Isin = item.SymbolISIN,
                    LicenseStartDate = item.StartDate,
                    LicenseEndDate = item.EndDate,
                    BuyOrderThreshold = item.MinimumStackedOrder,
                    SellOrderThreshold = item.MinimumStackedOrder,
                    TradeThreshold = item.MinimumDailyTrade,
                    BuyRange = float.Parse(((QuotedDomainTypes)item.QuotedDomainType).GetEnumDescription()),
                    SellRange = float.Parse(((QuotedDomainTypes)item.QuotedDomainType).GetEnumDescription()),
                    ContractTypeId = item.ContractTypeId,
                    FarasaRequestId = item.CreatedByRequestInstanceID,
                });
            }

            _logger.LogInformation($"Transformed {sourceData.Count()} records from source.");

            await _marketMakingCommandRepository
                .AddRangeAsync(targetList);


            _logger.LogInformation($"Added {sourceData.Count()} records to target.");


            _logger.LogInformation($"{_options.CurrentValue.JobId} ETL completed successfully.");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error => {ex.Message}");
            _logger.LogError($"Error => {ex.InnerException}");
            _logger.LogError($"Error => {ex.StackTrace}");
        }
        


        
    }
}

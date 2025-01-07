using FarasaEtl.Infrastructure;
using FarasaEtl.Infrastructure.Data;
using FarasaEtl.Infrastructure.Data.Hangfire;
using FarasaEtl.Infrastructure.Data.Target;
using Hangfire;
using FarasaEtl.Infrastructure.SystemLogs;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.CreateLog();

Logging.Information("THIS IS INFO");
Logging.Error("THIS IS ERROR");
// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration);

await builder.Services
    .MigrateDatabase<HangfireDbContext>(async (context, services) =>
    {
        await Task.CompletedTask;
    });

await builder.Services
    .MigrateDatabase<MonitoringBaseInfoDbContext>(async (context, services) =>
    {
        await Task.CompletedTask;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet(
    "/",
    () => { return Results.Redirect("/hangfire"); }
    );

app.UseHangfireDashboard();
app.StartRecurringJobs();

//app.MapGet(
//    "/",
//    async (IMarketMakingQueryRepository queryRepo, IMarketMakingCommandRepository commandRepo) =>
//    {
//        var sourceData = await queryRepo
//        .GetMarketMakingActivityLicensesAsync(0);
//        List<MarketMakingActivityLicense> targetList = new List<MarketMakingActivityLicense>();

//        foreach (var item in sourceData)
//        {
//            targetList.Add(new MarketMakingActivityLicense()
//            {
//                MarketMakerNationalId = item.NationalID.ToString(),
//                Isin = item.SymbolISIN,
//                LicenseStartDate = item.StartDate,
//                LicenseEndDate = item.EndDate,
//                BuyOrderThreshold = item.MinimumStackedOrder,
//                SellOrderThreshold = item.MinimumStackedOrder,
//                TradeThreshold = item.MinimumDailyTrade,
//                BuyRange = float.Parse(((QuotedDomainTypes)item.QuotedDomainType).GetEnumDescription()),
//                SellRange = float.Parse(((QuotedDomainTypes)item.QuotedDomainType).GetEnumDescription()),
//                ContractTypeId = item.ContractTypeId,
//                FarasaRequestId = item.CreatedByRequestInstanceID,
//            });
//        }

//        await commandRepo
//                .AddRangeAsync(targetList);
//    }
//    );

await app.RunAsync();





namespace FarasaEtl.Domain.Entities.Source;
public class MarketMakingActivityLicenseSourse : SourceBaseEntity
{
    public long NationalID { get; set; }

    public string SymbolISIN { get; set; }

    public long MinimumDailyTrade { get; set; }

    public long MinimumStackedOrder { get; set; }

    public int QuotedDomainType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public long CreatedByRequestInstanceID { get; set; }

    public short ContractTypeId { get; set; }

    public override string ToString()
    {
        return Id+ " "+ NationalID.ToString()+ " " + SymbolISIN + " " + QuotedDomainType + " " + CreatedByRequestInstanceID.ToString() + " " + ContractTypeId;
    }
}

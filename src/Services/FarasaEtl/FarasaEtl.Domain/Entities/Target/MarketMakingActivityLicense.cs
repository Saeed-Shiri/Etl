namespace FarasaEtl.Domain.Entities.Target
{
    public class MarketMakingActivityLicense : TargetBaseEntity
    {
        //شناسه ملی بازارگردان
        public string MarketMakerNationalId { get; set; }

        //کد ISIN نماد
        public string Isin { get; set; }

        //حداقل سفارش انباشته خرید 
        public long BuyOrderThreshold { get; set; }

        //حداقل سفارش انباشته فروش 
        public long SellOrderThreshold { get; set; }

        //حداقل معاملات روزانه
        public long TradeThreshold { get; set; }

        //دامنه مظنه خرید
        public float BuyRange { get; set; }

        //دامنه مظنه فروش
        public float SellRange { get; set; }

        //تاریخ شروع مجوز
        public DateTime LicenseStartDate { get; set; }

        //تاریخ پایان مجوز
        public DateTime LicenseEndDate { get; set; }


        //کد درخواست در فراسا
        public long FarasaRequestId { get; set; }


        //نوع قرارداد بازارگردانی
        //آغاز
        //تمدید
        //اتمام
        public short ContractTypeId { get; set; }

    }
}

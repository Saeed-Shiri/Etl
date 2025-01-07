using System.ComponentModel;


namespace FarasaEtl.Application.Enums
{
    public enum QuotedDomainTypes
    {
        [Description("0")]
        InValid = 0,
        //[Description("یک درصد کمتر از دامنه مجاز نوسان قیمت در تابلو مربوطه بازار پایه فرابورس ایران")]
        [Description("-1")]
        PayehMarket = 1,
        [Description("1")]
        MainMarket1 = 2,
        [Description("2")]
        MainMarket2 = 3,
        [Description("3")]
        MainMarket3 = 4,
        [Description("4")]
        MainMarket4 = 5,
        [Description("2.25")]
        MainMarket5 = 6,
        [Description("2.5")]
        MainMarket6 = 7,
        [Description("2.75")]
        MainMarket7 = 8,
        [Description("5")]
        MainMarket8 = 9,
    }
}

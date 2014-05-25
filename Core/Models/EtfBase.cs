
namespace Core.Models
{
    public abstract class EtfBase
    {
        // ETF NAME
        public string EtfName { get; set; }

        //TICKER
        public string Ticker { get; set; }

        //CATEGORY
        public string Category { get; set; }

        //FUND FAMILY
        public string FundFamily { get; set; }

        public abstract T LoadRow<T>(string[] rows) where T : class;
    }
}

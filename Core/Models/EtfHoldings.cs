// -----------------------------------------------------------------------
// <copyright file="EtfHoldings.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Core.Models
{
    public class EtfHoldings : EtfBase
    {
        //AVERAGE MARKET CAP					
        public string AverageMarketCap { get; set; }

        //PORTFOLIO P/E
        public string PortfolioPE { get; set; }

        //PORTFOLIO P/S
        public string PortfolioPS { get; set; }

        //PORTFOLIO PRICE/CASHFLOW	
        public string PortfolioPriceCashflow { get; set; }

        //PORTFOLIO PRICE/ BOOK
        public string PortfolioPriceBook { get; set; }

        //EARNINGS GROWTH RATE (TTM) - Trailing Twelce Months
        public string EarnignsGrowthRateTTM { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if (rows.Length < 10)
                throw new ArgumentException("requires 10 rows to be passed.");

            Date = DateTime.Now;
            EtfName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            AverageMarketCap = rows[5];
            PortfolioPE = rows[6];
            PortfolioPS = rows[7];
            PortfolioPriceCashflow = rows[8];
            PortfolioPriceBook = rows[9];
            EarnignsGrowthRateTTM = rows[10];

            return this as T;
        }
    }
}

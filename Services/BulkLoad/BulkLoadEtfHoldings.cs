// -----------------------------------------------------------------------
// <copyright file="BulkLoadEtfHoldings.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadEtfHoldings : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "AverageMarketCap", "PortfolioPE", "PortfolioPS",
                "PortfolioPriceCashflow", "PortfolioPriceBook", "EarnignsGrowthRateTTM"
            };

        public BulkLoadEtfHoldings(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithHoldings(IEnumerable<EtfHoldings> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                                + value.Category + "^" + value.FundFamily + "^"
                                + value.AverageMarketCap + "^" + value.PortfolioPE + "^"
                                + value.PortfolioPS + "^" + value.PortfolioPriceCashflow + "^"
                                + value.PortfolioPriceBook + "^" + value.EarnignsGrowthRateTTM;
                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

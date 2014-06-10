// -----------------------------------------------------------------------
// <copyright file="BulkLoadTradingVolume.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadTradingVolume : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "IntradayVolume", "ThreeMoAverageVolume", "LastTrade",
                "WeekHigh52", "WeekLow52"
            };

        public BulkLoadTradingVolume(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithTradingVolume(IEnumerable<EtfTradingVolume> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                                + value.Category + "^" + value.FundFamily + "^"
                                + value.IntradayVolume + "^" + value.ThreeMoAveragevolume + "^"
                                + value.LastTrade + "^" + value.WeekHigh52 + "^"
                                + value.WeekLow52;
                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

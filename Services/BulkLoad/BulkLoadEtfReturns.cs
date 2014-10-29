// -----------------------------------------------------------------------
// <copyright file="BulkLoadEtfReturns.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadEtfReturns : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "IntradayReturn", "ThreeMoReturn", "YTDReturn",
                "OneYrReturn", "ThreeYrReturn", "FiveYrReturn"
            };

        public BulkLoadEtfReturns(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithEtfReturns(IEnumerable<EtfReturns> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                    + value.Category + "^" + value.FundFamily + "^" 
                    + value.IntradayReturn + "^" + value.ThreeMoReturn + "^"
                             + value.YTDReturn + "^" + value.OneYrReturn
                             + "^" + value.ThreeYrReturn + "^" + value.FiveYrReturn;

                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
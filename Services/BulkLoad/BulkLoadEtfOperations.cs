// -----------------------------------------------------------------------
// <copyright file="BulkLoadEtfOperations.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadEtfOperations : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "NetAssets", "ExpenseRatio", "AnnualTurnoverRatio",
                "LegalType", "InceptionDate"
            };

        public BulkLoadEtfOperations(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithOperations(IEnumerable<EtfOperations> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                                + value.Category + "^" + value.FundFamily + "^"
                                + value.NetAssets + "^" + value.ExpenseRatio + "^"
                                + value.AnnualTurnoverRatio + "^" + value.LegalType + "^"
                                + value.InceptionDate;
                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

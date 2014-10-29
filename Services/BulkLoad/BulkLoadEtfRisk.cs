// -----------------------------------------------------------------------
// <copyright file="BulkLoadEtfRisk.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadEtfRisk : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "Beta3Yr", "Alpha3Yr", "RSquared3Yr"
            };

        public BulkLoadEtfRisk(ILogger logger) : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithRisk(IEnumerable<EtfRisks> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                                + value.Category + "^" + value.FundFamily + "^"
                                + value.Beta3Yr + "^" + value.Alpha3Yr + "^"
                                + value.RSquared3Yr;

                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

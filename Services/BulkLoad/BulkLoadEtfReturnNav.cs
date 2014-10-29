// -----------------------------------------------------------------------
// <copyright file="BulkLoadEtfReturnNav.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    internal class BulkLoadEtfReturnNav : BaseBulkLoad
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Date", "EtfName", "Ticker", "Category", "FundFamily",
                "ThreeMoReturn", "YTDReturn", "OneYrReturn", "FiveYrReturn"
            };

        public BulkLoadEtfReturnNav(ILogger logger)
            : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithReturnNav(IEnumerable<EtfReturnNavs> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Date + "^" + value.EtfName + "^" + value.Ticker + "^"
                    + value.Category + "^" + value.FundFamily + "^"
                    + value.ThreeMoReturn + "^" + value.YTDReturn + "^"
                             + value.OneYrReturn + "^" + value.FiveYrReturn;

                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('^');

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
/*
        public string ThreeMoReturn { get; set; }
        
        //YTD RETURN	
        public string YTDReturn { get; set; }
        
        //1-YR RETURN	
        public string OneYrReturn { get; set; }
        
        //5-YR RETURN
        public string FiveYrReturn { get; set; }
*/
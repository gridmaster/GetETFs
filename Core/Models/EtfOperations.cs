using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class EtfOperations : EtfBase
    {
        //				
        //NET ASSETS					
        public string NetAssets { get; set; }

        //EXPENSE RATIO
        public string ExpenseRatio { get; set; }

        //ANNUAL TURNOVER RATIO
        public string AnnualTurnoverRatio { get; set; }

        //LEGAL TYPE
        public string LegalType { get; set; }

        //INCEPTION DATE
        public DateTime InceptionDate { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if (rows.Length < 10)
                throw new ArgumentException("requires 10 rows to be passed.");

            EtfName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            NetAssets = rows[5];
            ExpenseRatio = rows[6];
            AnnualTurnoverRatio = rows[7];
            LegalType = rows[8];
            InceptionDate = Convert.ToDateTime(rows[9]);

            return this as T;
        }
    }
}

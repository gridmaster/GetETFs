using System;

namespace Core.Models
{
    public class EtfRisk : EtfBase
    {
        //BETA (3-YR)					
        public string Beta3Yr { get; set; }

        //ALPHA (3-YR)
        public string Alpha3Yr { get; set; }

        //R-SQUARED (3-YR)
        public string RSquared3Yr { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if (rows.Length < 8)
                throw new ArgumentException("requires 8 rows to be passed.");

            EtfName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            Beta3Yr = rows[5];
            Alpha3Yr = rows[6];
            RSquared3Yr = rows[7];

            return this as T;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="EtfReturn.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Core.Models
{
    public class EtfReturns : EtfBase
    {
        public EtfReturns()
        {
        }

        //INTRADAY RETURN	
        public string IntradayReturn { get; set; }
        
        //3-MO RETURN	
        public string ThreeMoReturn { get; set; }
        
        //YTD RETURN	
        public string YTDReturn { get; set; }
        
        //1-YR RETURN	
        public string OneYrReturn { get; set; }
        
        //3-YR RETURN	
        public string ThreeYrReturn { get; set; }
        
        //5-YR RETURN
        public string FiveYrReturn { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if(rows.Length < 11)
                throw new ArgumentException("requires 11 rows to be passed.");

            Date = DateTime.Now;
            EtfName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            IntradayReturn = rows[5];
            ThreeMoReturn = rows[6];
            YTDReturn = rows[7];
            OneYrReturn = rows[8];
            ThreeYrReturn = rows[9];
            FiveYrReturn = rows[10];

            return this as T;
        }
    }
}


using System;

namespace Core.Models
{
    public class ETFReturn : BaseETF
    {
        //INTRADAY RETURN	
        public string IntradayReturn { get; set; }
        
        //3-MO RETURN	
        public string ThreeMoReturn { get; set; }
        
        //YTD RETURN	
        public string YTDReturn { get; set; }
        
        //1-YR RETURN	
        public string OneYRReturn { get; set; }
        
        //3-YR RETURN	
        public string ThreeYRReturn { get; set; }
        
        //5-YR RETURN
        public string FiveYRReturn { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if(rows.Length < 11)
                throw new ArgumentException("requires 11 rows to be passed.");

            ETFName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            IntradayReturn = rows[5];
            ThreeMoReturn = rows[6];
            YTDReturn = rows[7];
            OneYRReturn = rows[8];
            ThreeYRReturn = rows[9];
            FiveYRReturn = rows[10];

            return this as T;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    //FUND NAME	TICKER	CATEGORY	FUND FAMILY	3-MO RETURN (NAV)	YTD RETURN (NAV)	1-YR RETURN (NAV)	5-YR RETURN (NAV)
    public class ETFReturnNAV : BaseETF
    {
        //3-MO RETURN	
        public string ThreeMoReturn { get; set; }
        
        //YTD RETURN	
        public string YTDReturn { get; set; }
        
        //1-YR RETURN	
        public string OneYRReturn { get; set; }
        
        //5-YR RETURN
        public string FiveYRReturn { get; set; }

        public override T LoadRow<T>(string[] rows)
        {
            if (rows.Length < 9)
                throw new ArgumentException("requires 9 rows to be passed.");

            ETFName = rows[1];
            Ticker = rows[2];
            Category = rows[3];
            FundFamily = rows[4];
            ThreeMoReturn = rows[5];
            YTDReturn = rows[6];
            OneYRReturn = rows[7];
            FiveYRReturn = rows[8];

            return this as T;
        }
    }
}

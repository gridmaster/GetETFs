﻿
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
    }
}

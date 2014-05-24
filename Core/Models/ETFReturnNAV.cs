using System;
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
    }
}

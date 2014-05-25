using System.Collections.Generic;
using Core.Models;

namespace Services.Interfaces
{
    public interface IEtfService
    {
        List<EtfReturn> GetReturnMkt();
        List<EtfReturnNav> GetReturnNav();
        List<EtfTradingVolume> GetTradingVolume();
        List<EtfHoldings> GetHoldings();
        List<EtfRisk> GetRisk();
        List<EtfOperations> GetOperations();
    }
}

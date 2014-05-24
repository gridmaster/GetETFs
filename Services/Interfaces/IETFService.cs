using System.Collections.Generic;
using Core.Models;

namespace Services.Interfaces
{
    public interface IETFService
    {
        void DoSomething(int id);
        List<ETFReturn> GetReturnMkt();
        List<ETFReturnNAV> GetReturnNav();
        List<ETFReturn> GetTradingVolumn();
        List<ETFReturn> GetHoldings();
        List<ETFReturn> GetRisk();
        List<ETFReturn> GetTradingOperations();
        string GetETFs(string uri);
    }
}

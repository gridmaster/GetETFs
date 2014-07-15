// -----------------------------------------------------------------------
// <copyright file="IEtfService.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Core.Models;

namespace Services.Interfaces
{
    public interface IEtfService
    {
        List<T> Get<T>(string uri) where T : EtfBase, new();
        List<EtfReturn> GetReturn();
        List<EtfReturnNav> GetReturnNav();
        List<EtfTradingVolume> GetTradingVolume();
        List<EtfHoldings> GetHoldings();
        List<EtfRisk> GetRisk();
        List<EtfOperations> GetOperations();
        bool SaveReturn();
        bool SaveReturnNav();
        bool SaveTradingVolume();
        bool SaveHoldings();
        bool SaveRisk();
        bool SaveOperations();
    }
}

using System.Collections.Generic;
using Core.Models;

namespace Services.Interfaces
{
    public interface IETFService
    {
        void DoSomething(int id);
        List<ETFReturn> GetReturnMkt();
        string GetETFs(string uri);
    }
}

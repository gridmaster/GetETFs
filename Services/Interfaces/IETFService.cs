﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IETFService
    {
        void DoSomething(int id);
        string GetETFs(string uri);
    }
}

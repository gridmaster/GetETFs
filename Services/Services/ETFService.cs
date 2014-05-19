using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using Services.Interfaces;

namespace Services.Services
{
    public class ETFService : BaseService, IETFService
    {
        public ETFService(ILogger logger) : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        public void DoSomething(int id)
        {
            ThrowIfNotInitialized();

            logger.DebugFormat("{0}write something here: {1}", Environment.NewLine, "I did something else...");

            logger.ErrorFormat("Unable to process {4}{0}surveyId: {1}{0}token: {2}{0}{3}"
                                    , Environment.NewLine
                                    , 1
                                    , id
                                    , "Other Message!"
                                    , GetThisMethodName());
        }
    }
}

using System;
using Core.Interface;
using Core.Models;
using DIContainer;
using GetETF.DIModule;
using Ninject;
using Services.Interfaces;

namespace GetETF
{
    internal class Program
    {
        #region initialize DI container

        private static void InitializeDiContainer()
        {
            NinjectSettings settings = new NinjectSettings
                {
                    LoadExtensions = false
                };

            // change DesignTimeModule to run other implementation ProductionModule || DebugModule
            IOCContainer.Instance.Initialize(settings, new DebugModule());
        }

        #endregion Initialize DI Container

        private static void Main(string[] args)
        {
            Console.WriteLine("{0}: Program startup...", DateTime.Now);

            try
            {
                InitializeDiContainer();

                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's runnin'...{0}", Environment.NewLine);

                string result = string.Empty;
                // List<T> Get<T>(string uri) where T : EtfBase, new();
                var ilist = IOCContainer.Instance.Get<IEtfService>().Get<EtfReturn>(EtfUris.uriReturn);

                var returnMkt = IOCContainer.Instance.Get<IEtfService>().SaveReturn();
                var returnNav = IOCContainer.Instance.Get<IEtfService>().SaveReturnNav();
                var returnTv = IOCContainer.Instance.Get<IEtfService>().SaveTradingVolume();
                var returnHoldings = IOCContainer.Instance.Get<IEtfService>().SaveHoldings();
                var returnRisk = IOCContainer.Instance.Get<IEtfService>().SaveRisk();
                var returnOperations = IOCContainer.Instance.Get<IEtfService>().SaveOperations();

                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Data collecting complete...{0}", Environment.NewLine);

            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", ex);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's done'...{0}", Environment.NewLine);
                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat(
                                "{0}********************************************************************************{0}",
                                Environment.NewLine);

                Console.ReadKey();
            }
        }
    }
}

using System;
using Core.Business;
using Core.Interface;
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
            string uri =
                "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab{0}&scol=imkt&stype=desc&rcnt={1}&page={2}";
            try
            {
                InitializeDiContainer();

                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat(
                                "{0}********************************************************************************{0}",
                                Environment.NewLine);
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's runnin'...{0}", Environment.NewLine);

                string result = string.Empty;

                var FullMonty = IOCContainer.Instance.Get<IETFService>().GetReturnMkt();

                Console.Write(result);
            }
            catch (Exception exc)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", exc);
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

        public static string GetPage(string url)
        {
            string result = GetWebPage.GetPage(url);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Business;
using Core.Interface;
using Core.Models;
using DIContainer;
using Ninject;
using Services.DIModule;
using Services.Interfaces;

namespace Services.Services
{
    public class ETFService : BaseService, IETFService
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

        public ETFService(ILogger logger) : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
            //InitializeDiContainer();
        }

        public void DoSomething(int id)
        {
            ThrowIfNotInitialized();

            logger.DebugFormat("{0}write something here: {1}", Environment.NewLine, "I did something...");

            logger.ErrorFormat("Unable to process {4}{0}surveyId: {1}{0}token: {2}{0}{3}"
                                    , Environment.NewLine
                                    , 1
                                    , id
                                    , "Other Message!"
                                    , GetThisMethodName());
        }

        public List<ETFReturn> GetReturnMkt()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnMkt's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab1&scol=imkt&stype=desc&rcnt={0}&page={1}";
            List<ETFReturn> etfList = GetETFList<ETFReturn>(uri);

            return etfList;
        }

        public List<ETFReturnNAV> GetReturnNav()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab2&scol=nav3m&stype=desc&rcnt={0}&page={1}";
            List<ETFReturnNAV> etfList = GetETFList<ETFReturnNAV>(uri);

            return etfList;
        }

        public List<ETFReturn> GetTradingVolumn()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingVolumn's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab3&scol=volint&stype=desc&rcnt={0}&page={1}";
            List<ETFReturnNAV> etfList = GetETFList<ETFTradingVolume>(uri);

            return etfList;
        }

        //public List<ETFReturn> GetHoldings()
        //{
        //    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetHoldings's runnin...{0}", Environment.NewLine);
        //    const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab4&scol=avgcap&stype=desc&rcnt={0}&page={1}";
        //    return GetETFList(uri);
        //}

        //public List<ETFReturn> GetRisk()
        //{
        //    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetRisk's runnin...{0}", Environment.NewLine);
        //    const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab5&scol=riskb&stype=desc&rcnt={0}&page={1}";
        //    return GetETFList(uri);
        //}

        //public List<ETFReturn> GetTradingOperations()
        //{
        //    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingOperations's runnin...{0}", Environment.NewLine);
        //    const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab6&scol=nasset&stype=desc&rcnt={0}&page={1}";
        //    return GetETFList(uri);
        //}

        private List<T> GetETFList<T>(string uri) where T : BaseETF, new()
        {
            List<T> etfList = new List<T>();
            
            try
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetETFs's runnin'...{0}", Environment.NewLine);

                int page = 1;
                bool notDone = true;
                string result = string.Empty;

                do
                {
                    string url = string.Format(uri, 100, page++);
                    result = GetETFs(url);

                    string xstrTable = WebWorks.GetTable(result);

                    xstrTable = xstrTable.Replace("<tr>", "~");
                    string[] rows = xstrTable.Split('~');
                    if (rows.Count() < 100) notDone = false;

                    foreach (var row in rows)
                    {
                        string[] getRows = WebWorks.GetColumns(row);
                        if (getRows.Count() > 6)
                        {
                            //var etfReturn = Activator.CreateInstance(typeof(T), new object[] { new string[], null }) as T;

                            T etfReturn = new T();
                            etfReturn.LoadRow<T>(getRows);
                            etfList.Add(etfReturn);
                        }
                    }
                } while (notDone);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Unable to get {1}{0}Error: {2}{0}{3}"
                        , Environment.NewLine
                        , uri
                        , ex.Message
                        , GetThisMethodName()); 
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetETFs's completed...{0}", Environment.NewLine);
            }

            return etfList;
        }

        public string GetETFs(string uri)
        {
            string webData = string.Empty;

            try
            {
                webData = WebWorks.GetResponse(uri);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Unable to get {1}{0}Error: {2}{0}{3}"
                                        , Environment.NewLine
                                        , uri
                                        , ex.Message
                                        , GetThisMethodName()); 
                webData = ex.Message;
            }

            return webData;
        }
    }
}

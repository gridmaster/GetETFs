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
            List<ETFReturn> etfList = new List<ETFReturn>();

            try
            {
                int page = 1;
                bool notDone = true;
                string result = string.Empty;

                do
                {
                    string url = string.Format(uri, 100, page++);
                    result = GetETFs(url);

                    string xstrTable = WebWorks.GetTable(result);

                    xstrTable = xstrTable.Replace("<tr>", "~");
                    var rows = xstrTable.Split('~');
                    if (rows.Count() < 100) notDone = false;

                    foreach (var row in rows)
                    {
                        string[] getRows = WebWorks.GetColumns(row);
                        if (getRows.Count() > 10)
                        {
                            ETFReturn etfReturn = new ETFReturn();
                            etfReturn.ETFName = getRows[1];
                            etfReturn.Ticker = getRows[2];
                            etfReturn.Category = getRows[3];
                            etfReturn.FundFamily = getRows[4];
                            etfReturn.IntradayReturn = getRows[5];
                            etfReturn.ThreeMoReturn = getRows[6];
                            etfReturn.YTDReturn = getRows[7];
                            etfReturn.OneYRReturn = getRows[8];
                            etfReturn.ThreeYRReturn = getRows[9];
                            etfReturn.FiveYRReturn = getRows[10];

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
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnMkt's completed...{0}", Environment.NewLine);
            }

            return etfList;
        }

        public List<ETFReturnNAV> GetReturnNav()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab2&scol=nav3m&stype=desc&rcnt={0}&page={1}";
            List<ETFReturnNAV> etfList = new List<ETFReturnNAV>();

            try
            {
                int page = 1;
                bool notDone = true;
                string result = string.Empty;

                do
                {
                    string url = string.Format(uri, 100, page++);
                    result = GetETFs(url);

                    string xstrTable = WebWorks.GetTable(result);

                    xstrTable = xstrTable.Replace("<tr>", "~");
                    var rows = xstrTable.Split('~');
                    if (rows.Count() < 100) notDone = false;

                    foreach (var row in rows)
                    {
                        string[] getRows = WebWorks.GetColumns(row);
                        if (getRows.Count() > 8)
                        {
                            ETFReturnNAV etfReturnNAV = new ETFReturnNAV();
                            etfReturnNAV.ETFName = getRows[1];
                            etfReturnNAV.Ticker = getRows[2];
                            etfReturnNAV.Category = getRows[3];
                            etfReturnNAV.FundFamily = getRows[4];
                            etfReturnNAV.ThreeMoReturn = getRows[5];
                            etfReturnNAV.YTDReturn = getRows[6];
                            etfReturnNAV.OneYRReturn = getRows[7];
                            etfReturnNAV.FiveYRReturn = getRows[8];

                            etfList.Add(etfReturnNAV);
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
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's completed...{0}", Environment.NewLine);
            }

            return etfList;
        }

        public List<ETFReturn> GetTradingVolumn()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingVolumn's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab3&scol=volint&stype=desc&rcnt={0}&page={1}";
            return GetETFList(uri);
        }

        public List<ETFReturn> GetHoldings()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetHoldings's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab4&scol=avgcap&stype=desc&rcnt={0}&page={1}";
            return GetETFList(uri);
        }

        public List<ETFReturn> GetRisk()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetRisk's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab5&scol=riskb&stype=desc&rcnt={0}&page={1}";
            return GetETFList(uri);
        }


        public List<ETFReturn> GetTradingOperations()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingOperations's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab6&scol=nasset&stype=desc&rcnt={0}&page={1}";
            return GetETFList(uri);
        }

        private List<ETFReturn> GetETFList(string uri)
        {
            List<ETFReturn> etfList = new List<ETFReturn>();
            
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
                    var rows = xstrTable.Split('~');
                    if (rows.Count() < 100) notDone = false;

                    foreach (var row in rows)
                    {
                        string[] getRows = WebWorks.GetColumns(row);
                        if (getRows.Count() > 10)
                        {
                            ETFReturn etfReturn = new ETFReturn();
                            etfReturn.ETFName = getRows[1];
                            etfReturn.Ticker = getRows[2];
                            etfReturn.Category = getRows[3];
                            etfReturn.FundFamily = getRows[4];
                            etfReturn.IntradayReturn = getRows[5];
                            etfReturn.ThreeMoReturn = getRows[6];
                            etfReturn.YTDReturn = getRows[7];
                            etfReturn.OneYRReturn = getRows[8];
                            etfReturn.ThreeYRReturn = getRows[9];
                            etfReturn.FiveYRReturn = getRows[10];

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

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Business;
using Core.Interface;
using Core.Models;
using DIContainer;
using Services.Interfaces;

namespace Services.Services
{
    public class EtfService : BaseService, IEtfService
    {
        #region Constructors
        public EtfService(ILogger logger) : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }
        #endregion Constructors

        #region IEtfService Implementation
        public List<EtfReturn> GetReturnMkt()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnMkt's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab1&scol=imkt&stype=desc&rcnt={0}&page={1}";
            List<EtfReturn> etfList = CallGetEtfAndCheckCounts<EtfReturn>(uri);
            
            return etfList;
        }

        public List<EtfReturnNav> GetReturnNav()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab2&scol=nav3m&stype=desc&rcnt={0}&page={1}";
            List<EtfReturnNav> etfList = CallGetEtfAndCheckCounts<EtfReturnNav>(uri);
            
            return etfList;
        }

        public List<EtfTradingVolume> GetTradingVolume()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingVolume's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab3&scol=volint&stype=desc&rcnt={0}&page={1}";
            List<EtfTradingVolume> etfList = CallGetEtfAndCheckCounts<EtfTradingVolume>(uri);

            return etfList;
        }

        public List<EtfHoldings> GetHoldings()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetHoldings's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab4&scol=avgcap&stype=desc&rcnt={0}&page={1}";
            List<EtfHoldings> etfList = CallGetEtfAndCheckCounts<EtfHoldings>(uri);

            return etfList;
        }

        public List<EtfRisk> GetRisk()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetRisk's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab5&scol=riskb&stype=desc&rcnt={0}&page={1}";
            List<EtfRisk> etfList = CallGetEtfAndCheckCounts<EtfRisk>(uri);

            return etfList;
        }

        public List<EtfOperations> GetOperations()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOperations's runnin...{0}", Environment.NewLine);
            const string uri = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf&tab=tab6&scol=nasset&stype=desc&rcnt={0}&page={1}";
            List<EtfOperations> etfList = CallGetEtfAndCheckCounts<EtfOperations>(uri);

            return etfList;
        }
        #endregion IEtfService Implementation

        #region Private Methods
        private List<T> CallGetEtfAndCheckCounts<T>(string uri) where T : EtfBase, new()
        {
            List<T> etfList = new List<T>();
            do
            {
                etfList = GetETFList<T>(uri);
            } while (etfList.Count < 1101);
            return etfList;
        }

        private List<T> GetETFList<T>(string uri) where T : EtfBase, new()
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
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetETFs's completed. {1} items returned. {0}", Environment.NewLine, etfList.Count);
            }

            return etfList;
        }

        private string GetETFs(string uri)
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
        #endregion Private Methods
    }
}

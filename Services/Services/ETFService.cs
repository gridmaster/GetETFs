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

            return GetEtfAndCheckCounts<EtfReturn>(EtfUris.uriReturn);
        }

        public List<EtfReturnNav> GetReturnNav()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfReturnNav>(EtfUris.uriReturnNav);
        }

        public List<EtfTradingVolume> GetTradingVolume()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingVolume's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfTradingVolume>(EtfUris.uriTradingVolume);
        }

        public List<EtfHoldings> GetHoldings()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetHoldings's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfHoldings>(EtfUris.uriHoldings);
        }

        public List<EtfRisk> GetRisk()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetRisk's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfRisk>(EtfUris.uriRisk);
        }

        public List<EtfOperations> GetOperations()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOperations's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfOperations>(EtfUris.uriOperations);
        }
        #endregion IEtfService Implementation

        #region Private Methods
        private List<T> GetEtfAndCheckCounts<T>(string uri) where T : EtfBase, new()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetEtfAndCheckCounts is runnin'...{0}", Environment.NewLine);

            List<T> etfList = new List<T>();

            do
            {
                etfList = GetEtfList<T>(uri);
            } while (etfList.Count < 1101);

            return etfList;
        }

        private List<T> GetEtfList<T>(string uri) where T : EtfBase, new()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetEtfList is runnin'...{0}", Environment.NewLine);

            List<T> etfList = new List<T>();
            
            try
            {
                int page = 1;
                bool notDone = true;

                do
                {
                    string result = GetETFs(string.Format(uri, 100, page++));

                    string[] rows = WebWorks.ExtractRowsFromWebPage(result);
                    notDone = rows.Count() > 99;

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
                logger.ErrorFormat("Unable to get {1}{0}Error: {2}{0}{3}", Environment.NewLine, uri, ex.Message, GetThisMethodName()); 
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetEtfList has completed. {1} Items returned. {0}", Environment.NewLine, etfList.Count);
            }

            return etfList;
        }

        private string GetETFs(string uri)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetETFs is fetching uri: {0}{1}{0}", Environment.NewLine, uri);

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

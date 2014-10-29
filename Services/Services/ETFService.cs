// -----------------------------------------------------------------------
// <copyright file="EtfService.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Business;
using Core.Interface;
using Core.Models;
using DIContainer;
using Services.BulkLoad;
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
        public List<T> Get<T>(string uri) where T : EtfBase, new()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}{1}'s runnin...{0}", Environment.NewLine, typeof(T).ToString());

            return GetEtfAndCheckCounts<T>(uri);
        }

        public List<EtfReturns> GetReturn()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturn's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfReturns>(EtfUris.uriReturn);
        }

        public List<EtfReturnNavs> GetReturnNav()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetReturnNav's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfReturnNavs>(EtfUris.uriReturnNav);
        }

        public List<EtfTradingVolumes> GetTradingVolume()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetTradingVolume's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfTradingVolumes>(EtfUris.uriTradingVolume);
        }

        public List<EtfHoldings> GetHoldings()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetHoldings's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfHoldings>(EtfUris.uriHoldings);
        }

        public List<EtfRisks> GetRisk()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetRisk's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfRisks>(EtfUris.uriRisk);
        }

        public List<EtfOperations> GetOperations()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOperations's runnin...{0}", Environment.NewLine);

            return GetEtfAndCheckCounts<EtfOperations>(EtfUris.uriOperations);
        }

        public bool SaveReturn()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveReturnMkt's runnin...{0}", Environment.NewLine);

            List<EtfReturns> etfReturn = GetEtfAndCheckCounts<EtfReturns>(EtfUris.uriReturn);

            var dt = IOCContainer.Instance.Get<BulkLoadEtfReturns>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadEtfReturns>().LoadDataTableWithEtfReturns(etfReturn, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithEtfReturns", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadEtfReturns>().BulkCopy<EtfReturns>(dt);
            }

            return success;
        }

        public bool SaveReturnNav()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveReturnNav's runnin...{0}", Environment.NewLine);

            List<EtfReturnNavs> etfReturnNav = GetEtfAndCheckCounts<EtfReturnNavs>(EtfUris.uriReturnNav);

            var dt = IOCContainer.Instance.Get<BulkLoadEtfReturnNav>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadEtfReturnNav>().LoadDataTableWithReturnNav(etfReturnNav, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithReturnNav", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadEtfReturnNav>().BulkCopy<EtfReturnNavs>(dt);
            }

            return success;
        }

        public bool SaveTradingVolume()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveTradingVolume's runnin...{0}", Environment.NewLine);

            List<EtfTradingVolumes> etfTradingVolume = GetEtfAndCheckCounts<EtfTradingVolumes>(EtfUris.uriTradingVolume);
            
            var dt = IOCContainer.Instance.Get<BulkLoadTradingVolume>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadTradingVolume>().LoadDataTableWithTradingVolume(etfTradingVolume, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithTradingVolume", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadTradingVolume>().BulkCopy<EtfTradingVolumes>(dt);
            }

            return success;
        }

        public bool SaveHoldings()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveHoldings's runnin...{0}", Environment.NewLine);

            List<EtfHoldings> etfHoldings = GetEtfAndCheckCounts<EtfHoldings>(EtfUris.uriHoldings);

            var dt = IOCContainer.Instance.Get<BulkLoadEtfHoldings>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadEtfHoldings>().LoadDataTableWithHoldings(etfHoldings, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithHoldings", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadEtfHoldings>().BulkCopy<EtfHoldings>(dt);
            }

            return success;
        }

        public bool SaveRisk()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveHoldings's runnin...{0}", Environment.NewLine);

            List<EtfRisks> etfRisk = GetEtfAndCheckCounts<EtfRisks>(EtfUris.uriRisk);

            var dt = IOCContainer.Instance.Get<BulkLoadEtfRisk>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadEtfRisk>().LoadDataTableWithRisk(etfRisk, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithRisk", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadEtfRisk>().BulkCopy<EtfRisks>(dt);
            }

            return success;
        }

        public bool SaveOperations()
        {
            bool success = false;
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}SaveOperations runnin...{0}", Environment.NewLine);

            List<EtfOperations> etfOperations = GetEtfAndCheckCounts<EtfOperations>(EtfUris.uriOperations);

            var dt = IOCContainer.Instance.Get<BulkLoadEtfOperations>().ConfigureDataTable();

            dt = IOCContainer.Instance.Get<BulkLoadEtfOperations>().LoadDataTableWithOperations(etfOperations, dt);

            if (dt == null)
            {
                logger.DebugFormat("{0}No data returned on LoadDataTableWithOperations", Environment.NewLine);
            }
            else
            {
                success = IOCContainer.Instance.Get<BulkLoadEtfOperations>().BulkCopy<EtfOperations>(dt);
            }

            return success;
        }
        #endregion IEtfService Implementation

        #region Private Methods
        private List<T> GetEtfAndCheckCounts<T>(string uri) where T : EtfBase, new()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetEtfAndCheckCounts is runnin'...{0}", Environment.NewLine);

            List<T> etfList = new List<T>();

            int count = GetETFCount(uri);

            do
            {
                etfList = GetEtfList<T>(uri);
            } while (etfList.Count < count);

            return etfList;
        }

        private int GetETFCount(string uri)
        {
            Regex regex = new Regex("((\\d+,?\\d+ Items))");
            string result = WebWorks.GetResponse(uri);

            var match = regex.Match(result);

            result = match.Groups[1].Value;

            result = result.Replace("Items", "").Replace(",", "").Trim();

            int n;
            bool isNumeric = int.TryParse(result, out n);
            return isNumeric ? n : 0;

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
                        string[] getColumns = WebWorks.GetColumns(row);
                        if (getColumns.Count() > 6)
                        {
                            T etfReturn = new T();
                            etfReturn.LoadRow<T>(getColumns);
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

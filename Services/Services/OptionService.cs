using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Business;
using Core.Interface;
using Core.Models;
using Core.Models.Options;
using DIContainer;
using Services.BulkLoad;
using Services.Interfaces;
using Services.SQL;

namespace Services.Services
{
// ReSharper disable SuggestUseVarKeywordEvident
    public class OptionService : BaseService, IOptionService
    {
        #region Constructors

        public OptionService(ILogger logger)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        #region IOptionService Implementation

        public bool GetOptions()
        {
            bool success = false;
            bool start = false;
            string startSymbol = ""; // "WSO";

            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOptions's runnin...{0}", Environment.NewLine);

            OptionList options = new OptionList();
            options.Options = new List<Options>();

            List<String> symbols = SymbolContext.GetSymbolList();

            foreach (string sym in symbols)
            {

                //if (sym != startSymbol && !start )
                //{
                //    continue;
                //}
                //else
                //{
                //    start = true;
                //}

                Dictionary<String, String> optDates = new Dictionary<string, string>();
                List<String> callRows = new List<String>();
                List<String> putRows = new List<String>();
                try
                {
                    int count = 0;
                    do
                    {
                        string webPage = GetWebPage(string.Format(OptionUris.UriBaseOption, sym));

                        if (webPage.IndexOf("There was an error retrieving options") > -1)
                        {
                            count++;
                            continue;
                        }

                        if (String.IsNullOrEmpty(webPage)) continue;

                        optDates = GetDates(ref webPage);

                        if (optDates == null)
                        {
                            count++;
                            continue;
                        }

                        if (optDates.Count == 0 && count < 10)
                        {
                            count++;
                            continue;
                        }
                        callRows = GetOptionRows(ref webPage);
                        putRows = GetOptionRows(ref webPage);
                        if (callRows.Count == 0 && putRows.Count == 0) count++;
                    } while (callRows.Count == 0 && putRows.Count == 0 && count < 10);
                }
                catch (Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOptions's GetWebPage Error: {0}{1}", ex.Message,
                        Environment.NewLine);
                }

                if (optDates == null) continue;

                for (int iNdx = 0; iNdx < optDates.Count; iNdx++)
                {
                    for (int i = 0; i < callRows.Count; i++)
                    {
                        try
                        {
                            Options opt = new Options()
                                {
                                    Symbol = sym,
                                    CallPut = "Call",
                                    DateCreated = DateTime.Now,
                                    ExperationDate = System.Convert.ToDateTime(optDates.ElementAt(iNdx).Value),
                                };
                            opt = LoadRow(opt, callRows[i]);
                            options.Options.Add(opt);
                        }
                        catch (Exception ex)
                        {
                            IOCContainer.Instance.Get<ILogger>()
                                        .InfoFormat("{0}GetOptions's Call Error: {0}{1}", ex.Message,
                                                    Environment.NewLine);
                        }
                    }

                    for (int i = 0; i < putRows.Count; i++)
                    {
                        try
                        {
                            Options opt = new Options()
                                {
                                    Symbol = sym,
                                    CallPut = "Put",
                                    DateCreated = DateTime.Now,
                                    ExperationDate = System.Convert.ToDateTime(optDates.ElementAt(iNdx).Value),
                                };
                            opt = LoadRow(opt, putRows[i]);
                            options.Options.Add(opt);
                        }
                        catch (Exception ex)
                        {
                            IOCContainer.Instance.Get<ILogger>()
                                        .InfoFormat("{0}GetOptions's Put Error: {0}{1}", ex.Message, Environment.NewLine);
                        }
                    }
                }
                try
                {
                    var dt = IOCContainer.Instance.Get<BulkLoadOptions>().ConfigureDataTable();

                    dt = IOCContainer.Instance.Get<BulkLoadOptions>().LoadDataTableWithOptions(options.Options, dt);

                    if (dt == null)
                    {
                        IOCContainer.Instance.Get<ILogger>()
                                    .InfoFormat("{0}No data returned on LoadDataTableWithOptions", Environment.NewLine);
                    }
                    else
                    {
                        success = IOCContainer.Instance.Get<BulkLoadOptions>().BulkCopy<Options>(dt, "OptionContext");
                        IOCContainer.Instance.Get<ILogger>()
                                    .InfoFormat("{0}BulkLoadOptions returned with: {1}", Environment.NewLine,
                                                success ? "Success" : "Fail");
                    }
                }
                catch (Exception ex)
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Bulk Load Options Error: {1}", Environment.NewLine, ex.Message);
                }
                finally
                {
                    options.Options = new List<Options>();
                }
            }

            return true;
        }

        #endregion IOptionService Implementation

        private Options LoadRow(Options option, string data)
        {
            try
            {
                option.InTheMoney = data.IndexOf("class=\"in-the-money", System.StringComparison.Ordinal) > -1 ? true : false;

                string strike =
                    data.Substring(data.IndexOf("strike=", System.StringComparison.Ordinal) + "strike=".Length);
                decimal value = 0.00M;
                string number = strike.Substring(0, strike.IndexOf("\"", System.StringComparison.Ordinal));
                bool result = decimal.TryParse(number, out value);
                option.Strike = value;
                data = data.Substring(data.IndexOf("q?s=", System.StringComparison.Ordinal) + "q?s=".Length);
                option.ContractName = data.Substring(0, data.IndexOf("\"", System.StringComparison.Ordinal));

                option.Last = GetDecimal(ref data);
                option.Bid = GetDecimal(ref data);
                option.Ask = GetDecimal(ref data);
                option.Change = GetDecimal(ref data);

                option.PercentChange = GetPercent(ref data);
                option.Volume = GetVolume(ref data);
                option.OpenInterest = GetDecimal(ref data);
                option.ImpliedVolatility = GetPercent(ref data);
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}LoadRow Error: {0}{1}", ex.Message, Environment.NewLine);
            }
            return option;
        }

        private decimal GetVolume(ref string data)
        {
            bool result = false;
            decimal value = 0.00M;

            try
            {
                data =
                    data.Substring(data.IndexOf("volume", System.StringComparison.Ordinal) + "option_entry Fz-m".Length);
                data = data.Substring(data.IndexOf(">", System.StringComparison.Ordinal) + 1);
                string number = data.Substring(0, data.IndexOf("<", System.StringComparison.Ordinal));
                result = decimal.TryParse(number, out value);

            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat("{0}GetDecimal Error: {0}{1}", ex.Message, Environment.NewLine);
            }

            return result ? value : 0.00m;
        }

        private decimal GetDecimal(ref string data)
        {
            bool result = false;
            decimal value = 0.00M;

            try
            {
                data =
                    data.Substring(data.IndexOf("option_entry Fz-m", System.StringComparison.Ordinal) +
                                   "option_entry Fz-m".Length);
                data = data.Substring(data.IndexOf(">", System.StringComparison.Ordinal) + 1);
                string number = data.Substring(0, data.IndexOf("<", System.StringComparison.Ordinal));
                result = decimal.TryParse(number, out value);
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetDecimal Error: {0}{1}", ex.Message, Environment.NewLine);
            }

            return result ? value : 0.00m;
        }

        private string GetPercent(ref string data)
        {
            string number = String.Empty;
            try
            {
                data =
                    data.Substring(data.IndexOf("option_entry Fz-m", System.StringComparison.Ordinal) +
                                   "option_entry Fz-m".Length);
                data = data.Substring(data.IndexOf(">", System.StringComparison.Ordinal) + 1);
                number = data.Substring(0, data.IndexOf("<", System.StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat("{0}GetDecimal Error: {0}{1}", ex.Message, Environment.NewLine);
            }

            return number;
        }

        private List<String> GetOptionRows(ref string options)
        {
            List<String> optionRows = new List<String>();

            try
            {
                Regex regex = new Regex("tr data-row=");
                String calls = options.Substring(1, options.IndexOf("</table>", System.StringComparison.Ordinal));
                int TableLength = calls.Length + "</table>".Length;
                var match = regex.Match(calls);

                if (match.Index == 0)
                {
                    match = regex.Match(options);
                    if (match.Index == 0)
                    {
                        return optionRows;
                    }
                    else
                    {
                        calls = options;
                    }
                }

                do
                {
                    calls = calls.Substring(match.Index - 1);
                    int ndx = calls.IndexOf("</tr>", System.StringComparison.Ordinal);
                    optionRows.Add(calls.Substring(0, ndx + "</tr>".Length));
                    calls = calls.Substring(ndx);
                    match = regex.Match(calls);
                } while (match.Length > 0);

                options = options.Substring(TableLength);
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat("{0}GetOptionRows Error: {0}{1}", ex.Message, Environment.NewLine);
            }

            return optionRows;
        }

        private Dictionary<String, String> GetDates(ref string options)
        {
            Dictionary<String, String> optDates = new Dictionary<string, string>();

            try
            {
                Regex regex = new Regex("<b class='SelectBox-Text '>.*</b>");

                var match = regex.Match(options);

                if (match.Length == 0) return null;

                regex = new Regex("<b class='SelectBox-Text '>");
                match = regex.Match(options);
                options = options.Substring(match.Index + match.Length);


                regex = new Regex("<option data-selectbox-link=");
                match = regex.Match(options);
                do
                {
                    options = options.Substring(match.Index + match.Length);
                    int ndx = options.IndexOf(">", System.StringComparison.Ordinal);
                    int ndx2 = options.IndexOf("value=", System.StringComparison.Ordinal) + "value=".Length;
                    if (options.IndexOf("<", System.StringComparison.Ordinal) - ndx - 1 < 0) return null;
                    optDates.Add(options.Substring(ndx2, ndx - ndx2),
                                 options.Substring(ndx + 1,
                                                   options.IndexOf("<", System.StringComparison.Ordinal) - ndx - 1));
                    match = regex.Match(options);
                } while (match.Length > 0);

            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>()
                            .InfoFormat("{0}GetOptionRows Error: {0}{1}", ex.Message, Environment.NewLine);
            }

            return optDates;
        }

        private List<Options> LoadOptions(string uri)
        {
            return new List<Options>();
        }
    }

    // ReSharper restore SuggestUseVarKeywordEvident
}
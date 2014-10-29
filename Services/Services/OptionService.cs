using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Business;
using Core.Interface;
using Core.Models;
using Core.Models.Options;
using DIContainer;
using Services.BulkLoad;
using Services.Interfaces;

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
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}GetOptions's runnin...{0}", Environment.NewLine);

            //symbols = GetSymbols();
            string options = GetWebPage(string.Format(OptionUris.UriBaseOption, "VXX"));

            Dictionary<String, String> optDates = GetDates(ref options);

            List<String> callRows = GetOptionRows(ref options);
            List<String> putRows = GetOptionRows(ref options);


            foreach (KeyValuePair<String, String> optdate in optDates)
            {
                var wtf = optdate.Key;
                var wtf2 = optdate.Value;
            }
            options = options;
            return true;
        }

        #endregion IOptionService Implementation
        private List<String> GetOptionRows(ref string options)
        {
            Regex regex = new Regex("tr data-row=");

            String calls = options.Substring(0, options.IndexOf("</table>", System.StringComparison.Ordinal));

            int TableLength = calls.Length;

            var match = regex.Match(calls);

            List<String> optionRows = new List<String>();
            do
            {
                calls = calls.Substring(match.Index - 1);
                int ndx = calls.IndexOf("</tr>", System.StringComparison.Ordinal);
                optionRows.Add(calls.Substring(0, ndx + "</tr>".Length));
                calls = calls.Substring(ndx);
                match = regex.Match(calls);
            } while (match.Length > 0);

            options = options.Substring(TableLength);
            return optionRows;
        }

        private Dictionary<String, String> GetDates(ref string options)
        {
            Regex regex = new Regex("<b class='SelectBox-Text '>.*</b>");

            var match = regex.Match(options);

            if (match.Length == 0) return null;

            regex = new Regex("<b class='SelectBox-Text '>");
            match = regex.Match(options);
            options = options.Substring(match.Index + match.Length);

            Dictionary<String, String> optDates = new Dictionary<string, string>();

            regex = new Regex("<option data-selectbox-link=");
            match = regex.Match(options);
            do
            {
                options = options.Substring(match.Index + match.Length);
                int ndx = options.IndexOf(">", System.StringComparison.Ordinal) + 1;
                optDates.Add(options.Substring(0, ndx - 1),
                             options.Substring(ndx, options.IndexOf("<", System.StringComparison.Ordinal) - ndx));
                match = regex.Match(options);
            } while (match.Length > 0);
            return optDates;
        }

        private List<Options> LoadOptions(string uri)
        {
            return new List<Options>();
        }
    }

    // ReSharper restore SuggestUseVarKeywordEvident
}
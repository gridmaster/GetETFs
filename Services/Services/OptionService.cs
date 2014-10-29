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

            string options = GetWebPage(string.Format(OptionUris.uriBaseOption, "VXX"));

            Regex regex = new Regex("<b class='SelectBox-Text '>.*</b>");

            var match = regex.Match(options);

            if (match.Length == 0) return false;

            regex = new Regex("<b class='SelectBox-Text '>");
            match = regex.Match(options);
            options = options.Substring(match.Index+match.Length);
            Dictionary<String, String> OptDates = new Dictionary<string, string>();
            regex = new Regex("<option data-selectbox-link=");
            match = regex.Match(options);
            do
            {
                options = options.Substring(match.Index + match.Length);
                int ndx = options.IndexOf(">", System.StringComparison.Ordinal) + 1;
                OptDates.Add(options.Substring(0, ndx-1),
                    options.Substring(ndx, options.IndexOf("<", System.StringComparison.Ordinal) - ndx));
                match = regex.Match(options);
            } while (match.Length > 0);

            foreach (var optdate in OptDates)
            {
                var wtf = optdate.Key;
                var wtf2 = optdate.Value;
            }
            options = options;
            return true;
        }
        #endregion IOptionService Implementation

        private List<Options> LoadOptions(string uri)
        {
            return new List<Options>();
        }
    }
}

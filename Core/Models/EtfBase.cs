// -----------------------------------------------------------------------
// <copyright file="EtfBase.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Core.Models
{
    public abstract class EtfBase
    {
        public DateTime Date { get; set; }

        // ETF NAME
        public string EtfName { get; set; }

        //TICKER
        public string Ticker { get; set; }

        //CATEGORY
        public string Category { get; set; }

        //FUND FAMILY
        public string FundFamily { get; set; }

        public abstract T LoadRow<T>(string[] rows) where T : class;
    }
}

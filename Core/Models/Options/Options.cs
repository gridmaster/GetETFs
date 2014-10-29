// -----------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Core.Models.Options
{
    public class Options : BaseOptions
    {
        public decimal Strike { get; set; }
        public string ContractName { get; set; }
        public decimal Last { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Change { get; set; }
        public string PercentChange { get; set; }
        public decimal Volume { get; set; }
        public decimal OpenInterest { get; set; }
        public string ImpliedVolatility { get; set; }
    }
}

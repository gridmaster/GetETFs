// -----------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace Core.Models.Options
{
    public abstract class BaseOptions
    {
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime ExperationDate { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

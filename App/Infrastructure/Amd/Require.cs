using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class Require
    {
        [JsonProperty("paths")]
        public Dictionary<string,string> Paths { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class Require
    {
        [JsonProperty("paths")]
        public Dictionary<string,string> Paths { get; set; }

        public Dictionary<string, Shim> Shims { get; set; }

        public class Shim
        {
            [JsonProperty("deps")]
            public List<string> Dependencies { get; set; }
            [JsonProperty("exports")]
            public string Exports { get; set; }
        }
    }
}
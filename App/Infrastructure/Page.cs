using System.Linq;
using Newtonsoft.Json;

namespace App.Infrastructure
{
    public class Page
    {
        public static PageHelper Helper { get; set; }
        
        public Page(string clientModule)
        {
            var path = "Client/" + clientModule;
            Script = path;
            Stylesheets = Helper.GetStylesheetUrls(path).ToArray();
        }

        public string HtmlFile { get; set; }
        [JsonProperty("parent")]
        public string Master { get; set; }
        public string Title { get; set; }
        public string Script { get; set; }
        [JsonProperty("stylesheets")]
        public string[] Stylesheets { get; set; }
        public string Language { get; set; }
        public object Data { get; set; }
    }
}
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
            Title = "MileageStats Sample Application";
            MasterPage = "/appframe";
            InitializationModule = "Client/Shared/init";
            HtmlFile = "app.html";
        }

        [JsonIgnore]
        public string HtmlFile { get; set; }
        [JsonIgnore]
        public string InitializationModule { get; set; }

        [JsonProperty("parent")]
        public string MasterPage { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        public string Script { get; set; }
        [JsonProperty("stylesheets")]
        public string[] Stylesheets { get; set; }
        public string Language { get; set; }
        public object Data { get; set; }
    }
}
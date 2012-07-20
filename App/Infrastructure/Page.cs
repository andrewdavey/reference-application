namespace App.Infrastructure
{
    public class Page
    {
        public string HtmlFile { get; set; }
        public string Title { get; set; }
        public string Script { get; set; }
        public string Stylesheet { get; set; }
        public object Data { get; set; }
    }
}
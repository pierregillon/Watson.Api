namespace Watson.Domain
{
    public struct HtmlLocation
    {
        public string FirstSelectedHtmlNodeXPath { get; set; }
        public string LastSelectedHtmlNodeXPath { get; set; }
        public int SelectedTextStartOffset { get; set; }
        public int SelectedTextEndOffset { get; set; }
    }
}
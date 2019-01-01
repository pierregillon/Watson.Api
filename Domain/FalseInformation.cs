namespace fakenewsisor.server
{
    public class FalseInformation
    {
        public string FirstSelectedHtmlNodeXPath { get; internal set; }
        public string LastSelectedHtmlNodeXPath { get; internal set; }
        public int SelectedTextStartOffset { get; internal set; }
        public int SelectedTextEndOffset { get; internal set; }
        public string SelectedText { get; internal set; }
    }
}
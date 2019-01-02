using System;

namespace fakenewsisor.server
{
    public class FactListItem
    {
        public string documentUrl { get; internal set; }
        public string text { get; internal set; }
        public string firstTextNodeXPath { get; internal set; }
        public string lastTextNodeXPath { get; internal set; }
        public int offsetStart { get; internal set; }
        public int offsetEnd { get; internal set; }
    }
}
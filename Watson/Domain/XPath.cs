using System;
using System.Linq;
using System.Text.RegularExpressions;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public class XPath
    {
        private static readonly Regex _xPathRegex = new Regex(@"^(?<id>\/\/\*\[\@id=\'.*\'\])?(?<path>\/[a-z]*\d*(\[\d*\])?)*\/text\(\)(\[\d*\])?$".Replace("\'", "\""));
        private static readonly string [] _textNodeTypes = new [] { "strong", "em", "a", "span", "text()" };

        public static XPath Parse(string xPathString)
        {
            if (string.IsNullOrEmpty(xPathString)) {
                throw new InvalidXPathFormat();
            }
            if (_xPathRegex.IsMatch(xPathString) == false) {
                throw new InvalidXPathFormat();
            }

            return new XPath(xPathString);
        }

        private readonly string _xPathString;
        private readonly string[] _hierarchy;

        private XPath(string xPathString)
        {
            _xPathString = xPathString;
            _hierarchy = xPathString.Split("/", StringSplitOptions.RemoveEmptyEntries);
        }

        public bool IsInSameParagraph(XPath other)
        {
            for (int i = 0; i < Math.Max(_hierarchy.Length, other._hierarchy.Length); i++) {
                var ancestor = _hierarchy.ElementAtOrDefault(i);
                var otherAncestor = other._hierarchy.ElementAtOrDefault(i);
                if (ancestor != otherAncestor) {
                    if (new [] { ancestor, otherAncestor }.Any(p => _textNodeTypes.Any(type => p == null || p.StartsWith(type)) == false)) {
                        return false;
                    }
                }
            }
            return true;
        }

        public override string ToString()
        {
            return _xPathString;
        }
    }
}
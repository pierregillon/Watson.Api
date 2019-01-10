using System;
using System.Linq;

namespace Watson.Domain
{
    public static class StringExtensions
    {
        private static readonly string[] _wordSeparators = new [] {" ", ".", ",", ";", ":", "?", "!", "/", "\\", "(", ")", "{", "}"};
        
        public static int WordCount(this string sentence)
        {
            return sentence
                .Split(_wordSeparators, int.MaxValue, StringSplitOptions.RemoveEmptyEntries)
                .Count();
        }
    }
}
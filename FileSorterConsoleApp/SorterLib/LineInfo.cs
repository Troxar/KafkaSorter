using System;

namespace FileSorterConsoleApp.SorterLib
{
    internal class LineInfo : IEquatable<LineInfo>
    {
        internal int Number { get; }
        internal string Word { get; }
        internal string Line { get; }

        public LineInfo(int number, string word, string line)
        {
            Number = number;
            Word = word;
            Line = line;
        }

        public bool Equals(LineInfo? other)
        {
            return other != null 
                    && Number == other.Number
                    && Word == other.Word;
        }
    }
}
using System;

namespace FileGeneratorConsoleApp
{
    internal class NoWordsException : ApplicationException
    {
        internal NoWordsException(string message) : base(message)
        {
            
        }
    }
}
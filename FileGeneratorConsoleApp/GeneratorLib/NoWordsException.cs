using System;

namespace FileGeneratorConsoleApp.GeneratorLib
{
    internal class NoWordsException : ApplicationException
    {
        internal NoWordsException(string message) : base(message)
        {
            
        }
    }
}
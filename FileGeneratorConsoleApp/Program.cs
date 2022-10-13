using System;
using System.Configuration;

namespace FileGeneratorConsoleApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Dictionaries reading...");
            
            var folderPath = ConfigurationManager.AppSettings.Get("DictionariesPath");
            var reader = new DictionariesReader(folderPath);
            var words = reader.Read();

            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
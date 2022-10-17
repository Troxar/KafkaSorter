using System;
using System.Configuration;
using FileSorterConsoleApp.SorterLib;

namespace FileSorterConsoleApp
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Sorter starting...");
            
            var unsortedFilePath = ConfigurationManager.AppSettings.Get("UnsortedFilePath");
            var resultFilePath = ConfigurationManager.AppSettings.Get("ResultFilePath");
            var bufferSize = int.Parse(ConfigurationManager.AppSettings.Get("BufferSize"));
            var config = new SorterConfig(unsortedFilePath, resultFilePath, bufferSize);
            ISorter sorter = new Sorter(config);
            sorter.Sort();
        }
    }
}
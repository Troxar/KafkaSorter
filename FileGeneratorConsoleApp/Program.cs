using System;
using System.Configuration;
using System.Threading;

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
            
            Console.WriteLine("Text generating...");
            
            var numberMinValue = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorNumberMinValue"));
            var numberMaxValue = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorNumberMaxValue"));
            var portion = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorPortion"));
            var tasksCount = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorTasksCount"));
            var resultFilePath = ConfigurationManager.AppSettings.Get("GeneratorResultFilePath");
            var resultFileSize = long.Parse(ConfigurationManager.AppSettings.Get("GeneratorResultFileSize"));
            var config = new FileGeneratorConfig(numberMinValue, numberMaxValue, words, 
                portion, tasksCount, resultFilePath, resultFileSize);
            var generator = new FileGenerator(config);
            generator.Generate(CancellationToken.None);
        }
    }
}
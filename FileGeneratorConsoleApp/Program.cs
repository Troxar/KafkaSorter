using System;
using System.Configuration;
using System.IO;
using System.Threading;
using FileGeneratorConsoleApp.GeneratorLib;

namespace FileGeneratorConsoleApp
{
    class Program
    {
        static void Main()
        {
            var words = ReadDictionaries();
            var filePath = GenerateFile(words);
            ProduceToKafka(filePath);
        }

        private static string[] ReadDictionaries()
        {
            Console.WriteLine("Dictionaries reading...");

            var folderPath = ConfigurationManager.AppSettings.Get("DictionariesPath");
            var reader = new DictionariesReader(folderPath);
            var words = reader.Read();
            
            return words;
        }
        
        private static string GenerateFile(string[] words)
        {
            Console.WriteLine("Text generating...");

            var folder = ConfigurationManager.AppSettings.Get("GeneratorResultFolder");
            var resultFilePath = Path.Combine(folder, Path.ChangeExtension(Path.GetRandomFileName(), "txt"));
            
            var numberMinValue = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorNumberMinValue"));
            var numberMaxValue = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorNumberMaxValue"));
            var portion = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorPortion"));
            var tasksCount = int.Parse(ConfigurationManager.AppSettings.Get("GeneratorTasksCount"));
            var resultFileSize = long.Parse(ConfigurationManager.AppSettings.Get("GeneratorResultFileSize"));
            
            var config = new FileGeneratorConfig(numberMinValue, numberMaxValue, words,
                portion, tasksCount, resultFilePath, resultFileSize);
            var generator = new FileGenerator(config);
            generator.Generate(CancellationToken.None);

            return resultFilePath;
        }

        private static void ProduceToKafka(string filePath)
        {
            Console.WriteLine("Kafka producing...");
            
            var bootstrapServers = ConfigurationManager.AppSettings.Get("KafkaBootstrapServers");
            var topic = ConfigurationManager.AppSettings.Get("KafkaTopic");

            var config = new KafkaProducerConfig(bootstrapServers, topic);
            var producer = new KafkaProducer(config);
            producer.Produce(filePath);
        }
    }
}
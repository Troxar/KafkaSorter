using System;
using System.Configuration;
using System.IO;
using FileSorterConsoleApp.SorterLib;

namespace FileSorterConsoleApp
{
    static class Program
    {
        static void Main()
        {
            ConsumeFromKafka();
        }

        private static void ConsumeFromKafka()
        {
            Console.WriteLine("Consume starting...");

            var bootstrapServers = ConfigurationManager.AppSettings.Get("KafkaBootstrapServers");
            var groupId = ConfigurationManager.AppSettings.Get("KafkaGroupId");
            var topic = ConfigurationManager.AppSettings.Get("KafkaTopic");

            var config = new KafkaConsumerConfig(bootstrapServers, groupId, topic, Sort);
            var consumer = new KafkaConsumer(config);
            consumer.Consume();
        }

        private static string Sort(string unsortedFilePath)
        {
            var resultFolder = ConfigurationManager.AppSettings.Get("ResultFolder");
            var resultFilePath = Path.Combine(resultFolder, Path.GetFileName(unsortedFilePath));
            
            var bufferSize = int.Parse(ConfigurationManager.AppSettings.Get("BufferSize"));
            var config = new SorterConfig(unsortedFilePath, resultFilePath, bufferSize);
            var sorter = new Sorter(config);
            sorter.Sort();

            return resultFilePath;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace FileSorterConsoleApp.SorterLib
{
    internal class KafkaConsumer
    {
        private readonly KafkaConsumerConfig _config;

        internal KafkaConsumer(KafkaConsumerConfig config)
        {
            _config = config;
        }

        internal void Consume()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = _config.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = true
            };
            
            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
            {
                consumer.Subscribe(_config.Topic);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, args) =>
                {
                    args.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (consumer.Consume(cts.Token) is { } result)
                    {
                        Task.Run(() => HandleConsumeResult(result), cts.Token);
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Consume error: {ex.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
        
        private void HandleConsumeResult(ConsumeResult<Null, string> result)
        {
            var unsortedFilePath = result.Message.Value; 
            Console.WriteLine($"File to sort: {unsortedFilePath}");
            
            try
            {
                var resultFilePath = _config.Function(unsortedFilePath);
                Console.WriteLine($"Sorted file: {resultFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
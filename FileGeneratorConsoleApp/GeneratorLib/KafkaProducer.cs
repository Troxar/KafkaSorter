using Confluent.Kafka;

namespace FileGeneratorConsoleApp.GeneratorLib
{
    internal class KafkaProducer
    {
        private readonly KafkaProducerConfig _config;
        
        internal KafkaProducer(KafkaProducerConfig config)
        {
            _config = config;
        }

        internal void Produce(string filePath)
        {
            var producer = new ProducerBuilder<Null, string>(
                    new ProducerConfig
                    {
                        BootstrapServers = _config.BootstrapServers,
                        Acks = Acks.Leader
                    })
                .Build();
            producer.Produce(_config.Topic, new Message<Null, string>
            {
                Value = filePath
            });
            producer.Flush();
        }
    }
}
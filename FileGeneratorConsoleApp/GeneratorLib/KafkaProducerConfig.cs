namespace FileGeneratorConsoleApp.GeneratorLib
{
    internal class KafkaProducerConfig
    {
        internal string BootstrapServers { get; }
        internal string Topic { get; }

        internal KafkaProducerConfig(string bootstrapServers, string topic)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
        }
    }
}
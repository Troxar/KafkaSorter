using System;

namespace FileSorterConsoleApp.SorterLib
{
    internal class KafkaConsumerConfig
    {
        internal string BootstrapServers { get; }
        internal string GroupId { get; }
        internal string Topic { get; }

        internal Func<string, string> Function { get; }

        internal KafkaConsumerConfig(string bootstrapServers, string groupId, string topic, Func<string, string> function)
        {
            BootstrapServers = bootstrapServers;
            GroupId = groupId;
            Topic = topic;
            Function = function;
        }
    }
}
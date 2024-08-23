

using ChapChap.Consumers.Data;

namespace ChapChap.Consumers
{
    public class ConsumersConfiguration
    {
        public MongoConfiguration MongoConfiguration { get; set; } = null!;

        public string ChannelAddress { get; set; } = string.Empty;
    }
}

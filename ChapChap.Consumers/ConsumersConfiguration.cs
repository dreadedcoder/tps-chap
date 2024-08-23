

using ChapChap.Consumers.Data;

namespace ChapChap.Consumers
{
    /// <summary>
    /// Configuration data for the services in the Consumers project
    /// </summary>
    public class ConsumersConfiguration
    {
        public MongoConfiguration MongoConfiguration { get; set; } = null!;

        public string ChannelAddress { get; set; } = string.Empty;
    }
}

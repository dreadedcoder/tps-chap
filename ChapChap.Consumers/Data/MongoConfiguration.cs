
namespace ChapChap.Consumers.Data
{
    /// <summary>
    /// Class for configuring Mongo Db for the consumers.
    /// </summary>
    public class MongoConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// defaults to 2
        /// </summary>
        public int MinConnectionPoolSize { get; set; } = 2;
        public string TransactionsCollectionName { get; set; } = string.Empty;

    }
}

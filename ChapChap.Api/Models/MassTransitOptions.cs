namespace ChapChap.Api.Models
{
    /// <summary>
    /// MassTransit configuration options
    /// </summary>
    public class MassTransitOptions
    {
        public RabbitMQ RabbitMQ { get; set; } = null!;
    }

    public class RabbitMQ
    {
        public string Host { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TransactionQueue { get; set; } = string.Empty;
    }
}

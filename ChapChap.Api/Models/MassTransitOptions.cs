namespace ChapChap.Api.Models
{
    public class MassTransitOptions
    {
        public RabbitMQ? RabbitMQ { get; set; }
    }

    public class RabbitMQ
    {
        public string Host { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TransactionQueue { get; set; } = string.Empty;
    }
}

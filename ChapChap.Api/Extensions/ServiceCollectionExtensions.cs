using MassTransit;
using ChapChap.Api.Models;
using ChapChap.Consumers;

namespace ChapChap.Api.Extensions
{
    /// <summary>
    /// Extensions to IServiceCollection to declutter program.cs.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MassTransit services.
        /// 
        /// Configures RabbitMQ and the transaction queue (ReceiveEndpoint).
        /// 
        /// Adds the <see cref="PaymentProcessingConsumer"/>.
        /// </summary>
        /// 
        /// <param name="services"> The  current <see cref="IServiceCollection"/> instance.</param>
        /// <param name="mtOptions"> The <see cref="MassTransitOptions"/> for configuring the broker. </param>
        /// 
        /// <returns> <see cref="IServiceCollection"/> for chaining method calls.</returns>
        /// 
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMassTransitConsumersWithRabbitMQ(
            this IServiceCollection services, MassTransitOptions mtOptions)
        {
            services
                .AddMassTransit(conf =>
                {
                    conf.AddConsumer<PaymentProcessingConsumer>();
                    conf.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(mtOptions.RabbitMQ.Host, c =>
                        {
                            c.Username(mtOptions.RabbitMQ.UserName);
                            c.Password(mtOptions.RabbitMQ.Password);
                        });

                        cfg.ReceiveEndpoint(mtOptions.RabbitMQ.TransactionQueue, configure =>
                        {
                            configure.Consumer<PaymentProcessingConsumer>(context);
                        });
                    });

                });

            return services;
        }
    }
}

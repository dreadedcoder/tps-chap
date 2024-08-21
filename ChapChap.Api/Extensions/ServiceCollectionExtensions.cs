using ChapChap.Api.Models;
using MassTransit;

namespace ChapChap.Api.Extensions
{
    /// <summary>
    /// Extensions to IServiceCollection to declutter program.cs
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MassTransit services.
        /// 
        /// Configures RabbitMQ and the transaction queue (ReceiveEndpoint)
        /// 
        /// </summary>
        /// 
        /// <param name="services"> The  current <see cref="IServiceCollection"/> instance</param>
        /// <param name="mtOptions"> The <see cref="MassTransitOptions"/> for configuring the broker </param>
        /// 
        /// <returns> <see cref="IServiceCollection"/> for chaining method calls</returns>
        /// 
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMassTransitWithRabbitMQ(
            this IServiceCollection services, MassTransitOptions mtOptions)
        {
            if(mtOptions.RabbitMQ == null)
                throw new ArgumentNullException(nameof(mtOptions.RabbitMQ));    

            services
                .AddMassTransit(conf =>
                {
                    conf.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(mtOptions.RabbitMQ.Host, c =>
                        {
                            c.Username(mtOptions.RabbitMQ.UserName);
                            c.Password(mtOptions.RabbitMQ.Password);
                        });

                        cfg.ReceiveEndpoint(mtOptions.RabbitMQ.TransactionQueue, configure =>
                        {
                            //configure.Consumer<object>(context);
                        });
                    });

                });

            return services;
        }
    }
}

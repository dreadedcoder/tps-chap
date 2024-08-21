using ChapChap.Api.Models;
using MassTransit;

namespace ChapChap.Api.Extensions
{
    public static class MassTransitServiceCollectionExtensions
    {
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

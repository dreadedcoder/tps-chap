using ChapChap.Consumers.Data;
using ChapChap.Consumers.gRPC;
using ChapChap.Consumers.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ChapChap.Consumers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures MongoDb, adds <see cref="TransactionRepository"/> and <see cref="IPaymentClient"/>.
        /// </summary>
        /// 
        /// <param name="services"> The <see cref="IServiceCollection"/> instance. </param>
        /// <param name="mongoConfig"> The mongo db configuration options. </param>
        /// 
        /// <returns>The <see cref="IServiceCollection"/> for chaining calls. </returns>
        public static IServiceCollection AddConsumersServices(this IServiceCollection services,
            ConsumersConfiguration config)
        {
            //configure database
            var mongoConfig = config.MongoConfiguration;
            services
                .AddSingleton<IMongoClient>(provider =>
                {
                    var mongoClientSettings = MongoClientSettings.FromConnectionString(
                        mongoConfig.ConnectionString);
                    mongoClientSettings.MinConnectionPoolSize = mongoConfig.MinConnectionPoolSize;

                    return new MongoClient(mongoClientSettings);
                }).AddSingleton(provider =>
                    provider.GetRequiredService<IMongoClient>().GetDatabase(mongoConfig.DatabaseName))
                .AddSingleton(provider =>
                    provider.GetRequiredService<IMongoDatabase>().GetCollection<Transaction>(
                        mongoConfig.TransactionsCollectionName)
                );

            //add other required services
            return services
                    .AddSingleton<ITransactionRepository, TransactionRepository>()
                    .AddSingleton(GrpcChannel.ForAddress(config.ChannelAddress))
                    .AddSingleton<IPaymentClient, PaymentClient>();
        }
    }
}

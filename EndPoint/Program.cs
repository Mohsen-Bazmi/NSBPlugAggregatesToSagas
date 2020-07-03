using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Raven.Client.Documents;

namespace EndPoint
{
    class Program
    {
        const string endpointName = "NSBPlugAggregatesToSagas.EndPoint";
        static async Task Main(string[] args)
        {
            Console.Title = endpointName;
            var epConfig = new EndpointConfiguration(endpointName);
            epConfig.UseTransport<RabbitMQTransport>()
                    .UseConventionalRoutingTopology()
                    .ConnectionString(connectionString: "host=localhost;username=guest;password=guest");
            var serialization = epConfig.UseSerialization<NewtonsoftSerializer>();
            serialization.Settings(new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
            epConfig.Conventions().DefiningEventsAs(type => type.Assembly == typeof(Domain.Events.UserRegistered).Assembly);

            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = endpointName,
            };
            var persistance = epConfig.UsePersistence<RavenDBPersistence>();
            persistance.DoNotSetupDatabasePermissions();
            store.Initialize();
            persistance.UseDocumentStoreForSagas(store);
            await store.EnsureDatabaseExistsAsync(endpointName, true).ConfigureAwait(false);

            epConfig.EnableInstallers();
            var endpoint = await Endpoint.Create(epConfig).ConfigureAwait(false);
            await endpoint.Start().ConfigureAwait(false);
            Console.ReadLine();



        }
    }
}

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

            var transport = epConfig.UseTransport<RabbitMQTransport>()
                                    .UseConventionalRoutingTopology()
                                    .ConnectionString(connectionString: "host=localhost;username=guest;password=guest");
            // var routing = transport.Routing();
            // routing.

            var serialization = epConfig.UseSerialization<NewtonsoftSerializer>();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            serialization.Settings(settings);


            epConfig.Conventions().DefiningEventsAs(type => type.Assembly == typeof(Domain.Events.UserRegistered).Assembly);


            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = endpointName,
            };
            var persistance = epConfig.UsePersistence<RavenDBPersistence>();
            persistance.DoNotSetupDatabasePermissions();
            documentStore.Initialize();
            using (documentStore.OpenSession(endpointName))
            {

            }
            persistance.SetDefaultDocumentStore(documentStore);


            epConfig.EnableInstallers();


            var endpoint = await Endpoint.Create(epConfig).ConfigureAwait(false);

            await endpoint.Start().ConfigureAwait(false);
            Console.ReadLine();



        }
    }
}

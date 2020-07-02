using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace EndPoint
{
    class Program
    {
        public static async Task EnsureDatabaseExistsAsync(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                await store.Maintenance.ForDatabase(database).SendAsync(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                try
                {
                    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                }
            }
        }
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


            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = endpointName,
            };
            var persistance = epConfig.UsePersistence<RavenDBPersistence>();
            persistance.DoNotSetupDatabasePermissions();
            store.Initialize();
            using (store.OpenSession(endpointName))
            {
            }
            await EnsureDatabaseExistsAsync(store, endpointName, true);
            // var compactSettings = new DatabaseRecord(endpointName);
            // var compactOperation = new CreateDatabaseOperation(compactSettings);
            // await store.Maintenance.Server.SendAsync(compactOperation).ConfigureAwait(false);

            persistance.SetDefaultDocumentStore(store);


            epConfig.EnableInstallers();


            var endpoint = await Endpoint.Create(epConfig).ConfigureAwait(false);

            await endpoint.Start().ConfigureAwait(false);
            Console.ReadLine();



        }
    }
}

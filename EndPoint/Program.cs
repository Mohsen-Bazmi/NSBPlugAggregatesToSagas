using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;

namespace EndPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NSBPlugAggregatesToSagas Endpoint");
            var epConfig = new EndpointConfiguration("NSBPlugAggregatesToSagas");
            
            var transport = epConfig.UseTransport<RabbitMQTransport>()
                                    .ConnectionString(connectionString: "host=localhost;username=guest;password=guest");
            var routing = transport.Routing();
            

            var serialization = epConfig.UseSerialization<NewtonsoftSerializer>();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            serialization.Settings(settings);


        }
    }
}

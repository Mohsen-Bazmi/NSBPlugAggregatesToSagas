using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NServiceBus;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        const string endpointName = "WebApiGateway.EndPoint";
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseNServiceBus(hostBuilderContext =>
               {
                   const string rabbitmqConnectionString = "host=localhost;username=guest;password=guest";
                   var epConfig = new EndpointConfiguration(endpointName);

                   epConfig.UseSerialization<NewtonsoftSerializer>();

                   var transport = epConfig.UseTransport<RabbitMQTransport>()
                                    .UseConventionalRoutingTopology()
                                    .ConnectionString(rabbitmqConnectionString);
                   var routing = transport.Routing();

                   routing.RouteToEndpoint(typeof(RegisterUser).Assembly, "NSBPlugAggregatesToSagas.EndPoint");
                   epConfig.Conventions().DefiningCommandsAs(t =>
                        t.Assembly == typeof(RegisterUser).Assembly
                        || t.Assembly == typeof(RenameUser).Assembly);


                //    epConfig.SendOnly();
                epConfig.EnableInstallers();


                   return epConfig;
               })
               .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    // logging.AddEventSourceLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

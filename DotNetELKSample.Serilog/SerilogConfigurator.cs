using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Microsoft.Extensions.Configuration;
using DotNetELKSample.Serilog.Configurations;
using Serilog.Sinks.Elasticsearch;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace DotNetELKSample.Serilog
{
    public static class SerilogConfigurator
    {
        private static Action<HostBuilderContext, LoggerConfiguration> Configure
            => (context, configuration) =>
            {
                var elasticConfiguration = context.Configuration.GetSection("ElasticApm").Get<ElasticApmConfiguration>();

                configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticConfiguration.ServerUrls))
                        {
                            IndexFormat = $"dotnetELKsampleApi-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                            NumberOfShards = 2,
                            NumberOfReplicas = 1,
                            ModifyConnectionSettings = config =>
                            {
                                config.BasicAuthentication(elasticConfiguration.User, elasticConfiguration.Password);
                                //config.ClientCertificate(new X509Certificate(certPath, password));
                                return config;
                            },
                            BatchAction = ElasticOpType.Create,
                            TypeName = null,
                            FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                     EmitEventFailureHandling.WriteToFailureSink |
                                     EmitEventFailureHandling.RaiseCallback
                        })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .ReadFrom.Configuration(context.Configuration);
            };

        public static IHostBuilder UseSerilog(this IHostBuilder builder)
        {
            builder.UseSerilog(Configure);
            return builder;
        }
    }
}

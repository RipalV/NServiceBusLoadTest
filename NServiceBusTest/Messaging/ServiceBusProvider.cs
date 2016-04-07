namespace NServiceBusTest.Messaging
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using NServiceBus;

    public interface IServiceBusProvider
    {
        IBus StartServiceBus();
    }

    public class ServiceBusProvider : IServiceBusProvider
    {
        private readonly IServiceBusConfig busConfig;

        private readonly ITransportProvider transportProvider;

        private readonly ILoggingConfigProvider loggingConfigProvider;

        public ServiceBusProvider(IServiceBusConfig busConfig, ITransportProvider transportProvider, ILoggingConfigProvider loggingConfigProvider)
        {
            this.busConfig = busConfig;
            this.transportProvider = transportProvider;
            this.loggingConfigProvider = loggingConfigProvider;
        }

        public IBus StartServiceBus()
        {
            this.loggingConfigProvider.Configure();
            var configuration = this.GetDefaultBusConfiguration();
            return Bus.Create(configuration).Start();
        }

        private static void SetLicense(BusConfiguration configuration)
        {
            var licenseText = ReadEmbeddedResource("NServiceBusTest.Messaging.License.License.xml");
            configuration.License(licenseText);
        }

        private static string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static void SetServiceBusConfig(BusConfiguration configuration, IServiceBusConfig busConfig)
        {
            configuration.EndpointName(busConfig.EndpointName);
            if (busConfig.AutofacContainer != null)
            {
                configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(busConfig.AutofacContainer));
            }
            configuration.CustomConfigurationSource(new CustomConfigurations(busConfig));

            busConfig.ConfigureConventions(configuration.Conventions());
        }

        private static void ConfigureAssembliesToScan(BusConfiguration configuration, IEnumerable<string> assemblyPatternsToScan)
        {
            IIncludesBuilder includesBuilder = AllAssemblies.Matching("NServiceBus.Core");
            if (assemblyPatternsToScan != null)
            {
                assemblyPatternsToScan.ToList().ForEach(assembly => includesBuilder = includesBuilder.And(assembly));
            }
            configuration.AssembliesToScan(includesBuilder);
        }

        private BusConfiguration GetDefaultBusConfiguration()
        {
            var configuration = new BusConfiguration();
            configuration.UseSerialization<JsonSerializer>();
            configuration.Transactions().DisableDistributedTransactions();

            if (this.busConfig.EnableInstallers)
            {
                configuration.EnableInstallers();
            }

            SetLicense(configuration);

            SetServiceBusConfig(configuration, this.busConfig);

            this.transportProvider.SetTransportConfiguration(configuration);

            IEnumerable<string> assemblyPatternsToScan = this.busConfig.AssemblyPatternsToScan ?? new List<string>();
            if (this.transportProvider.AssemblyPatternsToScan != null)
            {
                assemblyPatternsToScan = assemblyPatternsToScan.Concat(this.transportProvider.AssemblyPatternsToScan);
            }
            ConfigureAssembliesToScan(configuration, assemblyPatternsToScan);

            return configuration;
        }
    }
}
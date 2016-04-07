namespace NServiceBusTest.Messaging
{
    using System.Collections.Generic;

    using NServiceBus;
    using NServiceBus.Persistence;

    public interface ITransportProvider
    {
        IList<string> AssemblyPatternsToScan { get; }

        void SetTransportConfiguration(BusConfiguration configuration);
    }

    public class AzureServiceBusTransportProvider : ITransportProvider
    {
        private readonly IAzureServiceBusTransportConfig transportConfig;

        public AzureServiceBusTransportProvider(IAzureServiceBusTransportConfig transportConfig)
        {
            this.transportConfig = transportConfig;
            this.AssemblyPatternsToScan = new[] { "NServiceBus.Azure.Transports.WindowsAzureServiceBus.dll" };
        }

        public IList<string> AssemblyPatternsToScan { get; private set; }

        public void SetTransportConfiguration(BusConfiguration configuration)
        {
            configuration.ScaleOut().UseSingleBrokerQueue();

            configuration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString(this.transportConfig.AzureServiceBusConnectionString);

            configuration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString(this.transportConfig.AzureStorageConnectionString)
                .TableName(string.Format("{0}Subscriptions", this.transportConfig.StorageTablePrefix))
                .CreateSchema(true);

            configuration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
                .ConnectionString(this.transportConfig.AzureStorageConnectionString)
                .CreateSchema(true)
                .TimeoutManagerDataTableName("TimeoutManager")
                .TimeoutDataTableName(string.Format("{0}TimeoutData", this.transportConfig.StorageTablePrefix))
                .CatchUpInterval(3600)
                .PartitionKeyScope("yyyy-MM-dd-HH");
        }
    }
}
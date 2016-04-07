namespace NServiceBusTest.Messaging
{
    using System.Configuration;

    public interface IAzureServiceBusTransportConfig
    {
        string AzureServiceBusConnectionString { get; }

        string AzureStorageConnectionString { get; }

        string StorageTablePrefix { get; }
    }

    public class AzureServiceBusTransportConfig : IAzureServiceBusTransportConfig
    {
        public string AzureServiceBusConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["Messaging.AzureServiceBus.ConnectionString"];
            }
        }

        public string AzureStorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["Messaging.AzureStorage.ConnectionString"];
            }
        }

        public string StorageTablePrefix
        {
            get
            {
                return "Reports";
            }
        }
    }
}
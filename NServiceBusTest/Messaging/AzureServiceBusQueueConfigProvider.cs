namespace NServiceBusTest.Messaging
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    public class AzureServiceBusQueueConfigProvider : IProvideConfiguration<AzureServiceBusQueueConfig>
    {
        public AzureServiceBusQueueConfig GetConfiguration()
        {
            return new AzureServiceBusQueueConfig
                       {
                           BatchSize = EnvironmentConstants.BatchSize,
                           MaxDeliveryCount = EnvironmentConstants.MaxDeliveryCount,
                           LockDuration = EnvironmentConstants.LockDuration
                       };
        }
    }
}
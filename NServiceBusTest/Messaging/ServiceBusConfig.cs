namespace NServiceBusTest.Messaging
{
    using Autofac;

    public class ServiceBusConfig : ServiceBusConfigBase
    {
        public ServiceBusConfig()
            : base("NServiceBusTest", true)
        {
            this.AutofacContainer = BuildAutofacContainer();
        }

        public override int MaximumConcurrencyLevel
        {
            get
            {
                return EnvironmentConstants.BatchSize;
            }
        }

        public override IContainer AutofacContainer { get; protected set; }

        private static IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();

            return builder.Build();
        }
    }
}
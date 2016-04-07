namespace NServiceBusTest.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    internal class CustomConfigurations : IConfigurationSource
    {
        private readonly Dictionary<Type, object> configurations;

        public CustomConfigurations(IServiceBusConfig busConfig)
        {
            this.configurations = new Dictionary<Type, object>
                                 {
                                     { typeof(AuditConfig), CreateAuditConfig(busConfig.EndpointName) },
                                     { typeof(MessageForwardingInCaseOfFaultConfig), CreateMessageForwardingInCaseOfFaultConfig(busConfig.EndpointName) },
                                     { typeof(UnicastBusConfig), CreateMessageEndpointMappings(busConfig.AssemblyToEndpointMappings) },
                                     { typeof(TransportConfig), CreateTransportConfig(busConfig.MaximumConcurrencyLevel) }
                                 };
        }

        public T GetConfiguration<T>() where T : class, new()
        {
            var type = typeof(T);
            if (this.configurations.ContainsKey(type))
            {
                return (T)this.configurations[type];
            }

            Trace.TraceInformation("NServiceBus configuration for {0} is not available", type);
            return null;
        }

        private static TransportConfig CreateTransportConfig(int maximumConcurrencyLevel)
        {
            return new TransportConfig { MaximumConcurrencyLevel = maximumConcurrencyLevel };
        }

        private static AuditConfig CreateAuditConfig(string endpointName)
        {
            return new AuditConfig { QueueName = string.Format("{0}.Audit", endpointName) };
        }

        private static MessageForwardingInCaseOfFaultConfig CreateMessageForwardingInCaseOfFaultConfig(string endpointName)
        {
            return new MessageForwardingInCaseOfFaultConfig { ErrorQueue = string.Format("{0}.Error", endpointName) };
        }

        private static UnicastBusConfig CreateMessageEndpointMappings(IDictionary<string, string> assemblyToEndpointMappings)
        {
            var mappings = new MessageEndpointMappingCollection();
            if (assemblyToEndpointMappings != null)
            {
                foreach (var mapping in assemblyToEndpointMappings)
                {
                    mappings.Add(new MessageEndpointMapping { AssemblyName = mapping.Key, Endpoint = mapping.Value });
                }
            }
            return new UnicastBusConfig { MessageEndpointMappings = mappings };
        }
    }
}
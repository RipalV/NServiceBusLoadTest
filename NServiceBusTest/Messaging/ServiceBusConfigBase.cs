namespace NServiceBusTest.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Autofac;

    using NServiceBus;

    public interface IServiceBusConfig
    {
        string EndpointName { get; }

        int MaximumConcurrencyLevel { get; }

        IContainer AutofacContainer { get; }

        IDictionary<string, string> AssemblyToEndpointMappings { get; }

        IList<string> AssemblyPatternsToScan { get; }

        bool EnableInstallers { get; }

        void ConfigureConventions(ConventionsBuilder conventions);
    }

    public abstract class ServiceBusConfigBase : IServiceBusConfig
    {
        protected ServiceBusConfigBase(string endpointName, bool enableInstallers = true)
        {
            this.EndpointName = endpointName;
            this.EnableInstallers = enableInstallers;

            var contractAssemblies = this.GetContractsAssemblies();
            this.AssemblyToEndpointMappings = contractAssemblies.ToDictionary(assembly => assembly, assembly => assembly.Substring(0, assembly.Length - ".Contracts".Length));

            var assemblyPatternsToScan = new List<string> { endpointName + "." };
            assemblyPatternsToScan.AddRange(contractAssemblies);
            this.AssemblyPatternsToScan = assemblyPatternsToScan;
        }

        public void ConfigureConventions(ConventionsBuilder conventions)
        {
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains(".Contracts.Events"));
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains(".Contracts.Commands"));
        }

        public string EndpointName { get; private set; }

        public abstract int MaximumConcurrencyLevel { get; }

        public abstract IContainer AutofacContainer { get; protected set; }

        public IDictionary<string, string> AssemblyToEndpointMappings { get; private set; }

        public IList<string> AssemblyPatternsToScan { get; private set; }

        public bool EnableInstallers { get; private set; }

        private List<string> GetContractsAssemblies()
        {
            var appDomainDir = AppDomain.CurrentDomain.BaseDirectory;
            var binDir = Path.Combine(appDomainDir, "bin");
            var baseDir = Directory.Exists(binDir) ? binDir : appDomainDir; // required for tests to locate binaries

            var contractFiles = Directory.GetFiles(baseDir, "*.Contracts.dll").Select(Path.GetFileName).ToList();
            var result = contractFiles.Where(x => !x.StartsWith("Capabilities.", true, CultureInfo.InvariantCulture))
                .Select(x => x.Substring(0, x.Length - ".dll".Length))
                .ToList();
            return result;
        }
    }
}
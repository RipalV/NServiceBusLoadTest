[assembly: Microsoft.Owin.OwinStartup(typeof(NServiceBusTest.Startup))]

namespace NServiceBusTest
{
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using NServiceBus;

    using NServiceBusTest.Messaging;

    using Owin;

    public class Startup
    {
        public Startup()
        {
            var transportProvider = new AzureServiceBusTransportProvider(new AzureServiceBusTransportConfig());
            this.Bus = new ServiceBusProvider(new ServiceBusConfig(), transportProvider, new Log4NetConfigProvider()).StartServiceBus();
        }

        public IBus Bus { get; set; }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var container = BuildContainer();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);

            appBuilder.UseWebApi(config);
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterInstance(this.Bus).As<IBus>().SingleInstance();
            return builder.Build();
        }
    }
}
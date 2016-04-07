namespace NServiceBusTest.Messaging
{
    using NServiceBus.Log4Net;
    using NServiceBus.Logging;

    public interface ILoggingConfigProvider
    {
        void Configure();
    }

    public class Log4NetConfigProvider : ILoggingConfigProvider
    {
        public void Configure()
        {
            LogManager.Use<Log4NetFactory>();
        }
    }
}
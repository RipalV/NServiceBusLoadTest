namespace NServiceBusTest.Handlers
{
    using System;
    using System.Threading;

    using NServiceBus;

    using NServiceBusTest.Contracts.Commands;

    public class TestCommandHandler : IHandleMessages<Test>
    {
        public void Handle(Test message)
        {
            Thread.Sleep(TimeSpan.FromSeconds(message.SleepSeconds));
        }
    }
}
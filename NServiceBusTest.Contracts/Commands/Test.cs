namespace NServiceBusTest.Contracts.Commands
{
    using System;

    public class Test
    {
        public Guid RequestId { get; set; }

        public int Number { get; set; }

        public int SleepSeconds { get; set; }
    }
}

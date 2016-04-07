namespace NServiceBusTest.Controllers
{
    using System;
    using System.Web.Http;

    using NServiceBus;

    using NServiceBusTest.Contracts.Commands;

    public class StatusController : ApiController
    {
        private readonly IBus bus;

        public StatusController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpGet, Route("Status/Test/{count:int?}/{seconds:int?}")]
        public IHttpActionResult Test(int count = 100, int seconds = 30)
        {
            var startTime = DateTime.UtcNow;
            var requestId = Guid.NewGuid();
            for (int i = 1; i <= count; i++)
            {
                this.bus.Send(new Test { RequestId = requestId, Number = i, SleepSeconds = seconds });
            }

            return this.Ok((DateTime.UtcNow - startTime).TotalMilliseconds);
        }
    }
}
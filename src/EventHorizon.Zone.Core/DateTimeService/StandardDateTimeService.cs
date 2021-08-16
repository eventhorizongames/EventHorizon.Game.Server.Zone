namespace EventHorizon.Zone.Core.DateTimeService
{
    using System;

    using EventHorizon.Zone.Core.Model.DateTimeService;

    public class StandardDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}

namespace EventHorizon.Zone.Core.DateTimeService
{
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using System;

    public class StandardDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
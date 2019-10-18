using System;
using EventHorizon.Zone.Core.Model.DateTimeService;

namespace EventHorizon.Zone.Core.DateTimeService
{
    public class StandardDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
using System;

namespace EventHorizon.Zone.Core.Model.DateTimeService
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}

using System;

namespace EventHorizon.Schedule.Cron
{
    [Serializable]
    public enum CrontabFieldKind
    {
        Seconds,
        Minute,
        Hour,
        Day,
        Month,
        DayOfWeek
    }
}
namespace EventHorizon.Zone.Core.Model.Core
{
    using System;
    using System.Numerics;

    public struct LocationState
    {
        public static readonly string PROPERTY_NAME = "locationState";

        public bool CanMove { get; set; }
        public DateTime NextMoveRequest { get; set; }
        public Vector3 MoveToPosition { get; set; }
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }

        public static readonly LocationState NEW = new LocationState
        {
            CanMove = true,
            NextMoveRequest = default(DateTime),
            MoveToPosition = Vector3.Zero,
            CurrentZone = string.Empty,
            ZoneTag = string.Empty,
        };
    }
}
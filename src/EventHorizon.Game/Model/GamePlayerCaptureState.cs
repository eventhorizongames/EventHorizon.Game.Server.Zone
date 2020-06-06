namespace EventHorizon.Game.Model
{
    using System;
    using System.Collections.Generic;

    public struct GamePlayerCaptureState
    {
        public static readonly string PROPERTY_NAME = "gamePlayerCaptureState";

        public int Captures { get; set; }
        public IList<string> CompanionsCaught { get; set; }
        public DateTime EscapeCaptureTime { get; set; }
        public bool ShownTenSecondMessage { get; set; }
        public bool ShownFiveSecondMessage { get; set; }

        public static GamePlayerCaptureState New() => new GamePlayerCaptureState
        {
            CompanionsCaught = new List<string>(),
        };
    }
}

using System;

namespace EventHorizon.Game.Server.Zone.Core.Exceptions
{
    [Serializable]
    public class UnableToPingCoreServerException : Exception
    {
        public UnableToPingCoreServerException(string message)
            : base(message) { }
    }
}
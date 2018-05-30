using System;
using System.Runtime.Serialization;

namespace EventHorizon.Game.Server.Zone.Core.Exceptions
{
    [Serializable]
    public class UnableToRegisterWithCoreServerException : Exception
    {
        public UnableToRegisterWithCoreServerException(string message)
            : base(message) { }
    }
}
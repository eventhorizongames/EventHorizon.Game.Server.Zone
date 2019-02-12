using System;
using System.Runtime.Serialization;

namespace EventHorizon.Identity.Exceptions
{
    [Serializable]
    public class IdentityServerRequestException : Exception
    {
        public IdentityServerRequestException(string message, Exception ex)
            : base(message, ex) { }
    }
}
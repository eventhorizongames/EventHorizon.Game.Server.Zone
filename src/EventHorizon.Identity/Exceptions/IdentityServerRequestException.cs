namespace EventHorizon.Identity.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class IdentityServerRequestException : Exception
{
    public IdentityServerRequestException(string message, Exception ex)
        : base(message, ex) { }
}

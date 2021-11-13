namespace EventHorizon.Identity.Tests.TestUtil
{
    using System;

    using IdentityModel.Client;

    public class TokenResponseMock 
        : TokenResponse
    {
        public new string TryGet(string name)
        {
            return String.Empty;
        }
    }
}

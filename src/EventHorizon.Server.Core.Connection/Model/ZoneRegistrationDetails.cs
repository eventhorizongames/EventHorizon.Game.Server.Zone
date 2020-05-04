namespace EventHorizon.Server.Core.Connection.Model
{
    public struct ZoneRegistrationDetails
    {
        public string ServerAddress { get; set; }
        public string Tag { get; set; }

        public ZoneRegistrationDetails(
            string serverAddress, 
            string tag
        )
        {
            ServerAddress = serverAddress;
            Tag = tag;
        }
    }
}
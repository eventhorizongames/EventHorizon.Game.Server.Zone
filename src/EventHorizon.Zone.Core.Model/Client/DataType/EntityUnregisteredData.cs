namespace EventHorizon.Zone.Core.Model.Client.DataType
{
    public struct EntityUnregisteredData : IClientActionData
    {
        public long EntityId { get; set; }
    }
}
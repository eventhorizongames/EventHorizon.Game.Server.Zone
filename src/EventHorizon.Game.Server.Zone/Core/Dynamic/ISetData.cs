namespace EventHorizon.Game.Server.Zone.Core.Dynamic
{
    /// <summary>
    /// Use this interface to allow for dynamic creation of Object data mapping.  
    /// </summary>
    public interface ISetData
    {
        dynamic Data { set; }
    }
}
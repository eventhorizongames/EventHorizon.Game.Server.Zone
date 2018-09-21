namespace EventHorizon.Game.Server.Zone.Core.RandomNumber
{
    public interface IRandomNumberGenerator
    {
        int Next(int minValue, int maxValue);
    }
}
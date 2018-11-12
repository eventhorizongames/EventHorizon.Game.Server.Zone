namespace EventHorizon.Game.Server.Zone.External.RandomNumber
{
    public interface IRandomNumberGenerator
    {
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}
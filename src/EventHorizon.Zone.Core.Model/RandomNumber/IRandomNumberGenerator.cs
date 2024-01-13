namespace EventHorizon.Zone.Core.Model.RandomNumber;

public interface IRandomNumberGenerator
{
    int Next(int maxValue);
    int Next(int minValue, int maxValue);
}

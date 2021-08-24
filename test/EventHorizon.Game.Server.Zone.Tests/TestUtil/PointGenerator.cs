namespace EventHorizon.Game.Server.Zone.Tests.TestUtil
{
    using System;
    using System.Numerics;

    public static class PointGenerator
    {
        private static readonly Random RANDOM = new();

        public static Vector3 GetRandomPoint(int high, int low = 0)
        {
            return new Vector3(RANDOM.Next(low, high), RANDOM.Next(low, high), RANDOM.Next(low, high));
        }
    }
}

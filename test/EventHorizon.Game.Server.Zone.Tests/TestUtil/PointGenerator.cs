using System;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Tests.TestUtil
{
    public static class PointGenerator
    {
        static Random RANDOM = new Random();

        public static Vector3 GetRandomPoint(int high, int low = 0)
        {
            return new Vector3(RANDOM.Next(low, high), RANDOM.Next(low, high), RANDOM.Next(low, high));
        }
    }
}

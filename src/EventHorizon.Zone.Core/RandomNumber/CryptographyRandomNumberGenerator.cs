using System;
using System.Security.Cryptography;
using EventHorizon.Zone.Core.Model.RandomNumber;

namespace EventHorizon.Zone.Core.RandomNumber
{
    public class CryptographyRandomNumberGenerator : IRandomNumberGenerator
    {
        private static readonly RandomNumberGenerator RANDOM = RandomNumberGenerator.Create();

        public int Next(
            int maxValue
        )
        {
            return Next(
                0, 
                maxValue
            );
        }

        public int Next(
            int minValue, 
            int maxValue
        )
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return (int)System.Math.Floor(
                (minValue + ((double)maxValue - minValue) * NextDouble())
            );
        }

        private double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            RANDOM.GetBytes(
                data
            );
            var randUint = BitConverter.ToUInt32(
                data, 
                0
            );
            return randUint / (uint.MaxValue + 1.0);
        }

    }
}
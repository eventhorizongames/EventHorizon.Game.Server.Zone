using System.Numerics;
using EventHorizon.Zone.Core.Model.Graphics;
using EventHorizon.Zone.System.Particle.Model.Settings;
using Xunit;

namespace EventHorizon.Zone.System.Particle.Tests.Settings
{
    public class ParticleSettingsTests
    {
        [Fact]
        public void TestValidateParticleSettingsKeepsExpectedApi()
        {
            // Given
            var name = "name";
            var capacity = 1;
            var particleTexture = "particle-texture";

            var minEmitBox = new Vector3(1, 2, 3);
            var maxEmitBox = new Vector3(4, 5, 6);

            var color1 = new Color4 { R = 1, G = 2, B = 3, A = 4 };
            var color2 = new Color4 { R = 5, G = 6, B = 7, A = 8 };
            var colorDead = new Color4 { R = 9, G = 10, B = 11, A = 12 };

            var minSize = 2f;
            var maxSize = 3f;

            var minLifeTime = 4f;
            var maxLifeTime = 5f;

            var emitRate = 6f;

            var gravity = new Vector3(7, 8, 9);

            var direction1 = new Vector3(10, 11, 12);
            var direction2 = new Vector3(12, 14, 15);

            var minAngularSpeed = 7f;
            var maxAngularSpeed = 8f;

            var minEmitPower = 9f;
            var maxEmitPower = 10f;

            var updateSpeed = 11f;

            var blendMode = 12;

            // When
            var actual = new ParticleSettings
            {
                Name = name,
                Capacity = capacity,
                ParticleTexture = particleTexture,
                MinEmitBox = minEmitBox,
                MaxEmitBox = maxEmitBox,

                Color1 = color1,
                Color2 = color2,
                ColorDead = colorDead,

                MinSize = minSize,
                MaxSize = maxSize,

                MinLifeTime = minLifeTime,
                MaxLifeTime = maxLifeTime,

                EmitRate = emitRate,

                Gravity = gravity,

                Direction1 = direction1,
                Direction2 = direction2,

                MinAngularSpeed = minAngularSpeed,
                MaxAngularSpeed = maxAngularSpeed,

                MinEmitPower = minEmitPower,
                MaxEmitPower = maxEmitPower,

                UpdateSpeed = updateSpeed,
                BlendMode = blendMode,
            };

            // Then
            Assert.Equal(name, actual.Name);
            Assert.Equal(capacity, actual.Capacity);
            Assert.Equal(particleTexture, actual.ParticleTexture);
            
            Assert.Equal(minEmitBox, actual.MinEmitBox);
            Assert.Equal(maxEmitBox, actual.MaxEmitBox);

            Assert.Equal(color1, actual.Color1);
            Assert.Equal(color2, actual.Color2);
            Assert.Equal(colorDead, actual.ColorDead);

            Assert.Equal(minSize, actual.MinSize);
            Assert.Equal(maxSize, actual.MaxSize);

            Assert.Equal(minLifeTime, actual.MinLifeTime);
            Assert.Equal(maxLifeTime, actual.MaxLifeTime);

            Assert.Equal(emitRate, actual.EmitRate);

            Assert.Equal(gravity, actual.Gravity);

            Assert.Equal(direction1, actual.Direction1);
            Assert.Equal(direction2, actual.Direction2);

            Assert.Equal(minAngularSpeed, actual.MinAngularSpeed);
            Assert.Equal(maxAngularSpeed, actual.MaxAngularSpeed);

            Assert.Equal(minEmitPower, actual.MinEmitPower);
            Assert.Equal(maxEmitPower, actual.MaxEmitPower);

            Assert.Equal(updateSpeed, actual.UpdateSpeed);
            Assert.Equal(blendMode, actual.BlendMode);
        }
    }
}
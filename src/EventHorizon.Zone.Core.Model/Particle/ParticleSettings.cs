using System.Numerics;

namespace EventHorizon.Zone.Core.Model.Particle
{
    public struct ParticleSettings
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string ParticleTexture { get; set; }
        public Vector3 MinEmitBox { get; set; }
        public Vector3 MaxEmitBox { get; set; }

        public Color4 Color1 { get; set; }
        public Color4 Color2 { get; set; }
        public Color4 ColorDead { get; set; }

        public float MinSize { get; set; }
        public float MaxSize { get; set; }

        public float MinLifeTime { get; set; }
        public float MaxLifeTime { get; set; }

        public float EmitRate { get; set; }

        public Vector3 Gravity { get; set; }

        public Vector3 Direction1 { get; set; }
        public Vector3 Direction2 { get; set; }

        public float MinAngularSpeed { get; set; }
        public float MaxAngularSpeed { get; set; }

        public float MinEmitPower { get; set; }
        public float MaxEmitPower { get; set; }

        public float UpdateSpeed { get; set; }
        public int BlendMode { get; set; }
    }
}
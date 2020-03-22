namespace EventHorizon.Zone.Core.Model.Core
{
    using System;
    using System.Numerics;

    public struct TransformState
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Nullable<float> ScalingDeterminant { get; set; }
    }
}
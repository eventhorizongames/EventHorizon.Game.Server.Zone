namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using global::System;
    using global::System.Numerics;

    public class ClientEntityMetadataTypes
    {
        public static ClientEntityMetadataTypes TYPE_DETAILS = new ClientEntityMetadataTypes();
        public readonly Type assetId = typeof(string);
        public readonly Type dense = typeof(bool);
        public readonly Type densityBox = typeof(Nullable<Vector3>);
        public readonly Type resolveHeight = typeof(bool);
        public readonly Type heightOffset = typeof(long);
    }
}
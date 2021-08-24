namespace EventHorizon.Zone.System.ModelState
{
    public struct EntityModelState
    {
        public static readonly string PROPERTY_NAME = "modelState";
        public static readonly EntityModelState DEFAULT = new()
        {
            AnimationList = new string[0],
            Mesh = ModelMesh.DEFAULT
        };
        public string[] AnimationList { get; set; }
        public float? ScalingDeterminant { get; set; }
        public ModelMesh Mesh { get; set; }

        public bool IsValid()
        {
            return Mesh.IsValid();
        }
    }
}

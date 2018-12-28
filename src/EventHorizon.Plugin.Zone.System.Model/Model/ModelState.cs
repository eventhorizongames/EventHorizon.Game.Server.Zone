namespace EventHorizon.Plugin.Zone.System.Model.Model
{
    public struct ModelState
    {
        public static readonly string PROPERTY_NAME = "ModelState";
        public static readonly ModelState DEFAULT = new ModelState
        {
            AnimationList = new string[0],
            Mesh = ModelMesh.DEFAULT
        };
        public string[] AnimationList { get; set; }
        public ModelMesh Mesh { get; set; }

        public bool IsValid()
        {
            return Mesh.IsValid();
        }
    }
}
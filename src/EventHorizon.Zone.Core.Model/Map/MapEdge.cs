namespace EventHorizon.Zone.Core.Model.Map
{
    public struct MapEdge
    {
        public static MapEdge NULL = new(-1, -1);

        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public float Cost { get; set; }

        public MapEdge(
            int fromIndex,
            int toIndex
        )
        {
            FromIndex = fromIndex;
            ToIndex = toIndex;
            Cost = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var typedObj = (MapEdge)obj;
            return typedObj.FromIndex == FromIndex
                && typedObj.ToIndex == ToIndex;
        }

#pragma warning disable IDE0070 // Use 'System.HashCode' - Needs this to be deterministic
        public override int GetHashCode()
#pragma warning restore IDE0070 // Use 'System.HashCode'
        {
            int hash = 17;
            hash = hash * 31 + FromIndex.GetHashCode();
            hash = hash * 31 + ToIndex.GetHashCode();
            return hash;
        }
    }
}

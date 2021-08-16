namespace EventHorizon.Zone.Core.Model.Map
{
    public struct MapEdge
    {
        public static MapEdge NULL = new MapEdge(-1, -1);

        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public float Cost { get; set; }

        public MapEdge(
            int fromIndex,
            int toIndex
        )
        {
            this.FromIndex = fromIndex;
            this.ToIndex = toIndex;
            this.Cost = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var typedObj = (MapEdge)obj;
            return typedObj.FromIndex == this.FromIndex
                && typedObj.ToIndex == this.ToIndex;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.FromIndex.GetHashCode();
            hash = hash * 31 + this.ToIndex.GetHashCode();
            return hash;
        }
    }
}

using UnityEngine;

namespace TerrainGenerator.Generation.Surface.Structure
{
    public struct ChunkCoordinates
    {
        public readonly int x;
        public readonly int z;

        public ChunkCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
    }
}

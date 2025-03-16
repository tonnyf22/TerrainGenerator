using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunkLODInfo", menuName = "Terrain Generator/Chunk LOD Info")]
    public class ChunkLODInfo : ScriptableObject
    {
        public MeshFillType meshFillType;
        [Range(2, 512)]
        public int meshResolution;
        [Min(0)]
        public float maxDistance;
    }
}
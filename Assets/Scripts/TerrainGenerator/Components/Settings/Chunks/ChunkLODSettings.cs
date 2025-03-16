using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunkLODSettings", menuName = "Terrain Generator/Chunk LOD Settings")]
    public class ChunkLODSettings : ScriptableObject
    {
        public MeshFillType meshFillType;
        [Range(2, 512)]
        public int meshResolution = 2;
        [Min(0)]
        public float maxDistance;
        [Range(2, 64)]
        public int waterGridCoveringResolution = 2;
    }
}

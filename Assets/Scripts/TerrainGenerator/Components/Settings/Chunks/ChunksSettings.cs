using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunksSettings", menuName = "Terrain Generator/Chunks Settings")]
    public class ChunksSettings : ScriptableObject
    {
        [Range(0, 100.0f)]
        public float chunkSize;
        public ChunkLODInfo[] chunkLODInfos;
        public float chunkPreloadMaxDistance;
    }
}
using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunksSettings", menuName = "Terrain Generator/Chunks/Chunks Settings")]
    public class ChunksSettings : ScriptableObject
    {
        public float chunkSize;
        public ChunkLODSettings[] chunkLODSettings;
        public float chunkPreloadMaxDistance;
    }
}

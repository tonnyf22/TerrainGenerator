using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunksSettings", menuName = "Terrain Generator/Chunks Settings")]
    public class ChunksSettings : ScriptableObject
    {
        [Min(0)]
        public float chunkSize;
        [Expandable]
        public ChunkLODInfo[] chunkLODSettings;
        public float chunkPreloadMaxDistance;
    }
}

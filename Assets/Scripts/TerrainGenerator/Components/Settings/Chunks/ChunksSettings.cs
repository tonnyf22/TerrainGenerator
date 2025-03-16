using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunksSettings", menuName = "Terrain Generator/Chunks/Chunks Settings")]
    public class ChunksSettings : ScriptableObject
    {
        [Min(0)]
        public float chunkSize;
        [Expandable]
        public ChunkLODSettings[] chunkLODSettings;
        public float chunkPreloadMaxDistance;
    }
}

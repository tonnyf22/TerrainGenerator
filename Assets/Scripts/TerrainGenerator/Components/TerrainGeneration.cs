using TerrainGenerator.Components.Settings.Chunks;
using TerrainGenerator.Components.Settings.Biomes;
using TerrainGenerator.Generation.Management;
using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components
{
    [DisallowMultipleComponent]
    public class TerrainGeneration : MonoBehaviour
    {
        [Min(1E-2f)]
        public float scanTimeInterval;
        public string seed;
        public Transform generationCenter;
        public ChunksSettings chunksSettings;
        public BiomesSystemSettings biomesSystemSettings;

        // debug
        public Material materialSurface;
        public Material materialWater;

        private ChunksGenerator chunksGenerator;

        void Awake()
        {
            chunksGenerator = ChunksGenerator.CreateChunksGenerator(
                seed,
                generationCenter,
                chunksSettings,
                biomesSystemSettings,
                gameObject);
            
            // debug
            chunksGenerator.materialSurface = materialSurface;
            chunksGenerator.materialWater = materialWater;
        }

        private void ScanForChunksUpdates()
        {
            chunksGenerator.ScanForChunksUpdate();
        }

        void Start()
        {
            InvokeRepeating("ScanForChunksUpdates", 0.0f, scanTimeInterval);
        }
    }
}

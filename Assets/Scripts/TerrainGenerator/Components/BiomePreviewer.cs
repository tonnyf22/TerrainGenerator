using NaughtyAttributes;
using TerrainGenerator.Components.Settings.Biomes;
using TerrainGenerator.Components.Settings.Chunks;
using TerrainGenerator.Generation.Management;
using UnityEngine;

namespace TerrainGenerator.Components
{
    public class BiomePreviewer : MonoBehaviour
    {
        public string seed;
        [Min(1)]
        public int chunksPerQuadrantDimension = 2;

        [Expandable]
        public ChunksSettings chunksSettings;
        [Expandable]
        public BiomesSystemSettings biomesSystemSettings;

        public Material materialSurface;
        public Material materialWater;

        private BiomePreviewerChunksGenerator biomePreviewerChunksGenerator;

        void Awake()
        {
            biomePreviewerChunksGenerator = BiomePreviewerChunksGenerator.CreateBiomePreviewerChunksGenerator(
                seed,
                chunksSettings,
                biomesSystemSettings,
                gameObject);
            
            // debug
            biomePreviewerChunksGenerator.materialSurface = materialSurface;
            biomePreviewerChunksGenerator.materialWater = materialWater;
        }

        void Start()
        {
            biomePreviewerChunksGenerator.GenerateChunks(chunksPerQuadrantDimension);
        }
    }
}
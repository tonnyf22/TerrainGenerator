using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Biomes
{
    [CreateAssetMenu(fileName = "BiomesSystemSettings", menuName = "Terrain Generator/Biomes/Biomes System Settings")]
    public class BiomesSystemSettings : ScriptableObject
    {
        public float biomeGridCellSize;
        public float biomeSubgridCellSize;
        [Min(1)]
        public int displacementInterblendLevel = 1;
        [Min(1)]
        public int scatteringsInterblendLevel = 1;
        public BiomeSettings[] biomesSettings;
    }
}
using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Biomes
{
    [CreateAssetMenu(fileName = "BiomesSystemSettings", menuName = "Terrain Generator/Biomes/Biomes System Settings")]
    public class BiomesSystemSettings : ScriptableObject
    {
        public float biomeGridCellSize;
        public float biomeSubgridCellSize;
        public int scatteringsInterblendLevel = 1;
        public BiomeSettings[] biomesSettings;
    }
}
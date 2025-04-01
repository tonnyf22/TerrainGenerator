using TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Biomes
{
    [CreateAssetMenu(fileName = "BiomeSettings", menuName = "Terrain Generator/Biomes/Biome Settings")]
    public class BiomeSettings : ScriptableObject
    {
        public BiomeGraph biomeNodeGraph;
    }
}
using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components.Settings.Bioms
{
    [CreateAssetMenu(fileName = "BiomsSystemSettings", menuName = "Terrain Generator/Bioms/Bioms System Settings")]
    public class BiomsSystemSettings : ScriptableObject
    {
        public float biomGridCellSize;
        public float biomSubgridCellSize;
        [Range(1, 3)]
        public int scatteringsInterblendLevel = 1;
        [Expandable]
        public BiomSettings[] biomsSettings;
    }
}
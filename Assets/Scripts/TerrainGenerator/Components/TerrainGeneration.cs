using TerrainGenerator.Components.Settings.Bioms;
using TerrainGenerator.Components.Settings.Chunks;
using NaughtyAttributes;
using UnityEngine;

namespace TerrainGenerator.Components
{
    [DisallowMultipleComponent]
    public class TerrainGeneration : MonoBehaviour
    {
        [Header("General")]
        [HorizontalLine()]
        public string seed;
        public Transform generationCenter;

        [Space(20)]

        [Header("Chunks settings")]
        [HorizontalLine()]
        [Expandable]
        public ChunksSettings chunksSettings;

        [Space(20)]

        [Header("Bioms settings")]
        [HorizontalLine()]
        [Expandable]
        public BiomsSettings biomsSettings;
    }
}

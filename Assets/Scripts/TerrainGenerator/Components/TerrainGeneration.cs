using TerrainGenerator.Components.Settings.Bioms;
using TerrainGenerator.Components.Settings.Chunks;
using UnityEngine;

namespace TerrainGenerator.Components
{
    public class TerrainGeneration : MonoBehaviour
    {
        public string seed;
        public Transform generationCenter;
        public ChunksSettings chunksSettings;
        public BiomsSettings biomsSettings;
    }
}
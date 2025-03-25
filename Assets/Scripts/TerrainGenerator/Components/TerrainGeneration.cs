using TerrainGenerator.Components.Settings.Bioms;
using TerrainGenerator.Components.Settings.Chunks;
using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using TerrainGenerator.Generation.Structure;

namespace TerrainGenerator.Components
{
    [DisallowMultipleComponent]
    public class TerrainGeneration : MonoBehaviour
    {
        public string seed;
        public Transform generationCenter;
        public ChunksSettings chunksSettings;
        public BiomsSystemSettings biomsSystemSettings;

        public Dictionary<ChunkCoordinates, Chunk> chunks = new Dictionary<ChunkCoordinates, Chunk>();

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }
}

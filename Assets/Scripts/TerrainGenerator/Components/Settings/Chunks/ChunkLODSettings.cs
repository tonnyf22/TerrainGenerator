using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunkLODSettings", menuName = "Terrain Generator/Chunks/Chunk LOD Settings")]
    public class ChunkLODSettings : ScriptableObject
    {
        public float maxDistance;
        public MeshFillType meshFillType;
        public int meshResolution = 2;
        public bool isApplyCollision;
        public bool isApplyWaterCovering = true;
        public MeshFillType waterCoveringMeshFillType;
        public int waterCoveringMeshResolution = 2;
        public bool isApplyScattering = true;
        public bool isApplyScatteringSparcing;
        public int scatteringSparceLevel = 1;
    }
}

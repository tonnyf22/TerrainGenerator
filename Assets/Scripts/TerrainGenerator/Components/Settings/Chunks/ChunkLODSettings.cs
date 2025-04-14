using UnityEngine;

namespace TerrainGenerator.Components.Settings.Chunks
{
    [CreateAssetMenu(fileName = "ChunkLODSettings", menuName = "Terrain Generator/Chunks/Chunk LOD Settings")]
    public class ChunkLODSettings : ScriptableObject
    {
        public float maxDistance;
        [Space(20)]
        public MeshFillType meshFillType;
        [Min(2)]
        public int meshResolution = 2;
        public bool isApplyCollision;
        [Space(20)]
        public bool isApplyWaterCovering = true;
        public MeshFillType waterCoveringMeshFillType;
        [Min(2)]
        public int waterCoveringMeshResolution = 2;
        [Space(20)]
        public bool isApplyScattering = true;
        public bool isApplyScatteringSparcing;
        [Min(1)]
        public int scatteringSparseLevel = 1;
    }
}

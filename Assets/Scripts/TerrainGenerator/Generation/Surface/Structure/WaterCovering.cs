using UnityEngine;

namespace TerrainGenerator.Generation.Surface.Structure
{
    public class WaterCovering
    {
        public static WaterCovering CreateWaterCovering(int waterCoveringMeshResolution, GameObject parentGameObject)
        {
            return new WaterCovering(
                waterCoveringMeshResolution,
                parentGameObject
            );
        }

        public readonly int waterCoveringMeshResolution;
        public readonly GameObject waterCoveringGameObject;

        public WaterCovering(int waterCoveringMeshResolution, GameObject parentGameObject)
        {
            this.waterCoveringMeshResolution = waterCoveringMeshResolution;
            waterCoveringGameObject = new GameObject("WaterCovering");
            waterCoveringGameObject.transform.parent = parentGameObject.transform;
        }

        public void ApplyWaterCoveringMesh(Mesh mesh)
        {
            //
        }
    }
}
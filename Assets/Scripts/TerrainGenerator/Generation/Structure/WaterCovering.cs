using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class WaterCovering
    {
        public static WaterCovering CreateWaterCovering(bool isApplyWaterCovering, int waterCoveringMeshResolution, GameObject parentGameObject)
        {
            return new WaterCovering(
                isApplyWaterCovering,
                waterCoveringMeshResolution,
                parentGameObject
            );
        }

        public readonly bool isApplyWaterCovering;
        public readonly int waterCoveringMeshResolution;
        public readonly GameObject waterCoveringGameObject;

        public WaterCovering(bool isApplyWaterCovering, int waterCoveringMeshResolution, GameObject parentGameObject)
        {
            this.isApplyWaterCovering = isApplyWaterCovering;
            this.waterCoveringMeshResolution = waterCoveringMeshResolution;
            waterCoveringGameObject = new GameObject("WaterCovering");
            waterCoveringGameObject.transform.parent = parentGameObject.transform;
        }

        public void ApplyWaterCoveringMesh(Mesh mesh)
        {
            if (waterCoveringGameObject.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                meshFilter = waterCoveringGameObject.AddComponent<MeshFilter>();
            }
            if (waterCoveringGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer = waterCoveringGameObject.AddComponent<MeshRenderer>();
            }

            meshFilter.mesh = mesh;
            // meshRenderer.material = ...;
        }
    }
}
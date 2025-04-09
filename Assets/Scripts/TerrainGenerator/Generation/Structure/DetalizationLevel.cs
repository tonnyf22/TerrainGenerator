using System;
using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Chunks;
using UnityEditor;
using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class DetalizationLevel
    {
        public static DetalizationLevel CreateDetalizationLevel(int levelIndex, MeshFillType meshFillType, int meshResolution, /* bool isApplyCollision, */ GameObject parentGameObject)
        {
            return new DetalizationLevel(
                levelIndex,
                meshFillType,
                meshResolution,
                // isApplyCollision,
                parentGameObject
            );
        }

        public readonly int levelIndex;
        public readonly MeshFillType meshFillType;
        public readonly int meshResolution;
        // public readonly bool isApplyCollision;
        public readonly GameObject detalizationLevelGameObject;
        public WaterCovering waterCovering { get; private set; }
        public readonly Dictionary<int, Scattering> scatterings;

        public DetalizationLevel(int levelIndex, MeshFillType meshFillType, int meshResolution, /* bool isApplyCollision, */ GameObject parentGameObject)
        {
            this.levelIndex = levelIndex;
            this.meshFillType = meshFillType;
            this.meshResolution = meshResolution;
            // this.isApplyCollision = isApplyCollision;
            detalizationLevelGameObject = new GameObject("ChunkDetalizationLevel");
            detalizationLevelGameObject.transform.parent = parentGameObject.transform;
            SetDetalizationLevelGameObjectCoordinates();
            scatterings = new Dictionary<int, Scattering>();
        }

        private void SetDetalizationLevelGameObjectCoordinates()
        {
            detalizationLevelGameObject.transform.localPosition = Vector3.zero;
        }

        public void Show()
        {
            detalizationLevelGameObject.SetActive(true);
        }

        public void Hide()
        {
            detalizationLevelGameObject.SetActive(false);
        }

        public void AddWaterCovering(WaterCovering waterCovering)
        {
            if (this.waterCovering != null)
            {
                throw new ArgumentException($"Water covering already exists for this detalization level.");
            }
            else
            {
                this.waterCovering = waterCovering;
            }
        }

        public void RemoveWaterCovering()
        {
            if (this.waterCovering == null)
            {
                throw new ArgumentException($"Water covering does not exist for this detalization level.");
            }
            else
            {
                this.waterCovering = null;
            }
        }

        public void AddScattering(int index, Scattering scattering)
        {
            if (scatterings.ContainsKey(index))
            {
                throw new ArgumentException($"Scattering index {index} already exists for this detalization level.");
            }
            else
            {
                scatterings.Add(index, scattering);
            }
        }

        public void RemoveScattering(int index)
        {
            if (!scatterings.ContainsKey(index))
            {
                throw new ArgumentException($"Scattering index {index} does not exist for this detalization level.");
            }
            else
            {
                scatterings.Remove(index);
            }
        }

        public void ApplySurfaceMesh(Mesh mesh, Material material)
        {
            if (!detalizationLevelGameObject.TryGetComponent(out MeshFilter meshFilter))
            {
                meshFilter = detalizationLevelGameObject.AddComponent<MeshFilter>();
            }
            if (!detalizationLevelGameObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer = detalizationLevelGameObject.AddComponent<MeshRenderer>();
            }

            meshFilter.mesh = mesh;
            meshRenderer.material = material;
        }

        public Mesh GetSurfaceMesh()
        {
            if (!detalizationLevelGameObject.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                throw new ArgumentException($"Mesh filter does not exist on this detalization level game object.");
            }
            else
            {
                return detalizationLevelGameObject.GetComponent<MeshFilter>().mesh;
            }
        }

        public void ApplySurfaceCollision(Mesh mesh)
        {
            if (!detalizationLevelGameObject.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider = detalizationLevelGameObject.AddComponent<MeshCollider>();
            }

            meshCollider.sharedMesh = mesh;
        }
    }
}
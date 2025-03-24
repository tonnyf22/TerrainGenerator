using System;
using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Chunks;
using UnityEngine;

namespace TerrainGenerator.Generation.Surface.Structure
{
    public class DetalizationLevel
    {
        public static DetalizationLevel CreateDetalizationLevel(MeshFillType meshFillType, int meshResolution, GameObject parentGameObject)
        {
            return new DetalizationLevel(
                meshFillType,
                meshResolution,
                parentGameObject
            );
        }

        public readonly MeshFillType meshFillType;
        public readonly int meshResolution;
        public readonly GameObject detalizationLevelGameObject;
        public readonly WaterCovering waterCovering;
        public readonly Dictionary<int, Scattering> scatterings;

        public DetalizationLevel(MeshFillType meshFillType, int meshResolution, GameObject parentGameObject)
        {
            this.meshFillType = meshFillType;
            this.meshResolution = meshResolution;
            detalizationLevelGameObject = new GameObject("ChunkDetalizationLevel");
            detalizationLevelGameObject.transform.parent = parentGameObject.transform;
        }

        public void Show()
        {
            detalizationLevelGameObject.SetActive(true);
        }

        public void Hide()
        {
            detalizationLevelGameObject.SetActive(false);
        }

        public void AddScattering(int index, Scattering scattering)
        {
            if (scatterings.ContainsKey(index))
            {
                throw new ArgumentException($"Scattering index {index} is already exists for this detalization level.");
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
                throw new ArgumentException($"Scattering index {index} is not exists for this detalization level.");
            }
            else
            {
                scatterings.Remove(index);
            }
        }

        public void ApplyMesh(Mesh mesh)
        {
            //
        }

        public void ApplyCollision(Mesh mesh)
        {
            // 
        }
    }
}
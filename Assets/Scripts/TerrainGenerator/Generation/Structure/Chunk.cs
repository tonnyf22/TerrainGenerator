using System;
using System.Collections.Generic;
using TerrainGenerator.Generation.Surface;
using TerrainGenerator.Generation.Water;
using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class Chunk
    {
        public static ChunkCoordinates LocatePointInChunkAsChunkCoordinates(float x, float z, float chunkSize)
        {
            return new ChunkCoordinates(
                Mathf.FloorToInt(x / chunkSize),
                Mathf.FloorToInt(z / chunkSize)
            );
        }

        public static Vector3 LocateChunkCoordinatesAsPoint(ChunkCoordinates chunkCoordinates, float chunkSize)
        {
            return new Vector3(
                chunkCoordinates.x * chunkSize,
                0.0f,
                chunkCoordinates.z * chunkSize
            );
        }

        public static Vector3 LocateCenterOfChunkAsPoint(ChunkCoordinates chunkCoordinates, float chunkSize)
        {
            return new Vector3(
                (chunkCoordinates.x + 0.5f) * chunkSize,
                0.0f,
                (chunkCoordinates.z + 0.5f) * chunkSize
            );
        }

        public static Vector3 LocatePointInChunkAsPoint(float x, float z, ChunkCoordinates chunkCoordinates, float chunkSize)
        {
            return new Vector3(
                chunkCoordinates.x * chunkSize + x,
                0.0f,
                chunkCoordinates.z * chunkSize + z
            );
        }

        public static Vector3 LocatePointInChunkAsPoint(float x, float y, float z, ChunkCoordinates chunkCoordinates, float chunkSize)
        {
            return new Vector3(
                chunkCoordinates.x * chunkSize + x,
                y,
                chunkCoordinates.z * chunkSize + z
            );
        }

        public static Chunk CreateChunk(float chunkSize, ChunkCoordinates chunkCoordinates, GameObject parentGameObject)
        {
            return new Chunk(chunkSize, chunkCoordinates, parentGameObject);
        }

        public readonly float chunkSize;
        public readonly ChunkCoordinates chunkCoordinates;
        public readonly GameObject chunkGameObject;
        public readonly Dictionary<int, DetalizationLevel> detalizationLevels;
        public SurfaceMeshGenerator surfaceMeshGenerator { get; private set; }
        public SurfaceDisplacementGenerator surfaceDisplacementGenerator { get; private set; }
        public WaterMeshGenerator waterMeshGenerator { get; private set; }

        public Chunk(float chunkSize, ChunkCoordinates chunkCoordinates, GameObject parentGameObject)
        {
            this.chunkSize = chunkSize;
            this.chunkCoordinates = chunkCoordinates;
            chunkGameObject = new GameObject("TerrainChunk");
            chunkGameObject.transform.parent = parentGameObject.transform;
            SetChunkGameObjectCoordinates();
            detalizationLevels = new Dictionary<int, DetalizationLevel>();
        }

        private void SetChunkGameObjectCoordinates()
        {
            chunkGameObject.transform.localPosition = new Vector3(
                chunkSize * chunkCoordinates.x,
                0.0f,
                chunkSize * chunkCoordinates.z
            );
        }

        public void AddSurfaceMeshGenerator(SurfaceMeshGenerator surfaceMeshGenerator)
        {
            if (this.surfaceMeshGenerator != null)
            {
                throw new ArgumentException($"Surface mesh generator already exists for this chunk.");
            }
            else
            {
                this.surfaceMeshGenerator = surfaceMeshGenerator;
            }
        }

        public void RemoveSurfaceMeshGenerator()
        {
            if (this.surfaceMeshGenerator == null)
            {
                throw new ArgumentException($"Surface mesh generator does not exist for this chunk.");
            }
            else
            {
                this.surfaceMeshGenerator = null;
            }
        }

        public void AddSurfaceDisplacementGenerator(SurfaceDisplacementGenerator surfaceDisplacementGenerator)
        {
            if (this.surfaceDisplacementGenerator != null)
            {
                throw new ArgumentException($"Surface displacement generator already exists for this chunk.");
            }
            else
            {
                this.surfaceDisplacementGenerator = surfaceDisplacementGenerator;
            }
        }

        public void RemoveSurfaceDisplacementGenerator()
        {
            if (this.surfaceDisplacementGenerator == null)
            {
                throw new ArgumentException($"Surface displacement generator does not exist for this chunk.");
            }
            else
            {
                this.surfaceDisplacementGenerator = null;
            }
        }

        public void AddWaterMeshGenerator(WaterMeshGenerator waterMeshGenerator)
        {
            if (this.waterMeshGenerator != null)
            {
                throw new ArgumentException($"Water mesh generator already exists for this chunk.");
            }
            else
            {
                this.waterMeshGenerator = waterMeshGenerator;
            }
        }

        public void RemoveWaterMeshGenerator()
        {
            if (this.waterMeshGenerator == null)
            {
                throw new ArgumentException($"Water mesh generator does not exist for this chunk.");
            }
            else
            {
                this.waterMeshGenerator = null;
            }
        }

        public void AddDetalizationLevel(int levelIndex, DetalizationLevel detalizationLevel)
        {
            if (detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} already exists for this chunk.");
            }
            else
            {
                detalizationLevels.Add(levelIndex, detalizationLevel);
            }
        }

        public DetalizationLevel GetDetalizationLevel(int levelIndex)
        {
            if (!detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} does not exist for this chunk.");
            }
            else
            {
                return detalizationLevels[levelIndex];
            }
        }

        public void RemoveDetalizationLevel(int levelIndex)
        {
            if (!detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} does not exist for this chunk.");
            }
            else
            {
                detalizationLevels.Remove(levelIndex);
            }
        }

        public void ShowDetalizationLevel(int levelIndex)
        {
            if (!detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} does not exist for this chunk.");
            }
            else
            {
                detalizationLevels[levelIndex].Show();
            }
        }

        public void HideDetalizationLevel(int levelIndex)
        {
            if (!detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} does not exist for this chunk.");
            }
            else
            {
                detalizationLevels[levelIndex].Hide();
            }
        }
    }
}

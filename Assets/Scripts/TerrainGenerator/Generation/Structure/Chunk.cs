using System;
using System.Collections.Generic;
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

        public static Chunk CreateChunk(float chunkSize, ChunkCoordinates chunkCoordinates, GameObject parentGameObject)
        {
            return new Chunk(chunkSize, chunkCoordinates, parentGameObject);
        }

        public readonly float chunkSize;
        public readonly ChunkCoordinates chunkCoordinates;
        public readonly GameObject chunkGameObject;
        public readonly Dictionary<int, DetalizationLevel> detalizationLevels;

        public Chunk(float chunkSize, ChunkCoordinates chunkCoordinates, GameObject parentGameObject)
        {
            this.chunkSize = chunkSize;
            this.chunkCoordinates = chunkCoordinates;
            chunkGameObject = new GameObject("TerrainChunk");
            SetChunkGameObjectCoordinates();
            chunkGameObject.transform.parent = parentGameObject.transform;
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

        public void AddDetalizationLevel(int levelIndex, DetalizationLevel detalizationLevel)
        {
            if (detalizationLevels.ContainsKey(levelIndex))
            {
                throw new ArgumentException($"Detalization level index {levelIndex} is already exists for this chunk.");
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
                throw new ArgumentException($"Detalization level index {levelIndex} is not exists for this chunk.");
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
                throw new ArgumentException($"Detalization level index {levelIndex} is not exists for this chunk.");
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
                throw new ArgumentException($"Detalization level index {levelIndex} is not exists for this chunk.");
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
                throw new ArgumentException($"Detalization level index {levelIndex} is not exists for this chunk.");
            }
            else
            {
                detalizationLevels[levelIndex].Hide();
            }
        }
    }
}

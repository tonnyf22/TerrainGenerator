using System;
using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Biomes.Scattering;
using TerrainGenerator.Generation.Structure;
using UnityEngine;

namespace TerrainGenerator.Generation.Scattering
{
    public class ScatteringPointsGenerator
    {
        public static ScatteringPointsGenerator CreateScatteringPointsGenerator(Chunk chunk, string seed)
        {
            return new ScatteringPointsGenerator(
                chunk,
                seed);
        }

        public readonly Chunk chunk;
        public readonly string seed;
        private DeterministicRandom deterministicRandom;

        public ScatteringPointsGenerator(Chunk chunk, string seed)
        {
            this.chunk = chunk;
            this.seed = seed;
            deterministicRandom = new DeterministicRandom(seed);
        }

        public List<Vector3> CreatePoints(ObjectsScattering objectsScattering, BiomeScatteringSettings biomeScatteringSettings)
        {
            switch (biomeScatteringSettings.scatteringType)
            {
                case ScatteringType.Random:
                    return GeneratePointsRandom(
                        objectsScattering.scatteringSparseLevel,
                        biomeScatteringSettings as BiomeRandomScatteringSettings);
                case ScatteringType.GridBased:
                    return GeneratePointsGridBased(
                        objectsScattering.scatteringSparseLevel,
                        biomeScatteringSettings as BiomeGridBasedScatteringSettings);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(biomeScatteringSettings.scatteringType),
                        biomeScatteringSettings.scatteringType,
                        "Unsupported ScatteringType.");
            }
        }

        private List<Vector3> GeneratePointsRandom(int scatteringSparseLevel, BiomeRandomScatteringSettings biomeRandomScatteringSettings)
        {
            List<Vector3> points = new List<Vector3>();

            int pointsNumber = biomeRandomScatteringSettings.targetResolution * biomeRandomScatteringSettings.targetResolution;
            int missedPoints = scatteringSparseLevel / 2;

            for (int index = 0; index < pointsNumber; index++)
            {
                if (missedPoints < scatteringSparseLevel)
                {
                    missedPoints++;
                }
                else
                {
                    missedPoints = 0;

                    CreateAndStorePointRandom(points, index);
                }
            }

            return points;
        }

        private List<Vector3> GeneratePointsGridBased(int scatteringSparseLevel, BiomeGridBasedScatteringSettings biomeGridBasedScatteringSettings)
        {
            List<Vector3> points = new List<Vector3>();

            int missedPoints = scatteringSparseLevel / 2;

            for (int xIndex = 0; xIndex < biomeGridBasedScatteringSettings.targetResolution; xIndex++)
            {
                for (int zIndex = 0; zIndex < biomeGridBasedScatteringSettings.targetResolution; zIndex++)
                {
                    if (missedPoints < scatteringSparseLevel)
                    {
                        missedPoints++;
                    }
                    else
                    {
                        missedPoints = 0;

                        CreateAndStorePointGridBased(
                            points,
                            xIndex,
                            zIndex,
                            biomeGridBasedScatteringSettings.targetResolution,
                            biomeGridBasedScatteringSettings.randomStep,
                            biomeGridBasedScatteringSettings.isApplyStepRange,
                            biomeGridBasedScatteringSettings.randomStepMin);
                    }
                }
            }

            return points;
        }

        private void CreateAndStorePointRandom(List<Vector3> points, int index)
        {
            float rawOffsetX = deterministicRandom.Value01(chunk.chunkCoordinates.x, index, chunk.chunkCoordinates.z);
            float rawOffsetZ = deterministicRandom.Value01(chunk.chunkCoordinates.z, chunk.chunkCoordinates.x, index);

            float x = chunk.chunkSize * rawOffsetX;
            float z = chunk.chunkSize * rawOffsetZ;

            Vector3 point = Chunk.LocatePointInChunkAsPoint(x, z, chunk.chunkCoordinates, chunk.chunkSize);

            points.Add(point);
        }

        private void CreateAndStorePointGridBased(List<Vector3> points, int xIndex, int zIndex, int targetResolution, float randomStep, bool isApplyStepRange, float randomStepMin)
        {
            float verticesGapSize = chunk.chunkSize / (targetResolution - 1);

            float rawRotationStep = deterministicRandom.Value01(
                chunk.chunkCoordinates.x * xIndex,
                chunk.chunkCoordinates.z * zIndex,
                xIndex * zIndex);

            float step;
            if (isApplyStepRange)
            {
                float rawOffsetStep = deterministicRandom.Value01(
                    xIndex * zIndex,
                    chunk.chunkCoordinates.z * zIndex,
                    chunk.chunkCoordinates.x * xIndex);
                step =
                    randomStepMin +
                    rawOffsetStep *
                        (randomStep -
                        randomStepMin);
            }
            else
            {
                step = randomStep;
            }

            float offsetStepX = Mathf.Cos(2 * Mathf.PI * rawRotationStep) * step;
            float offsetStepZ = Mathf.Sin(2 * Mathf.PI * rawRotationStep) * step;

            float x = xIndex * verticesGapSize + offsetStepX;
            float z = zIndex * verticesGapSize + offsetStepZ;
            
            if (Chunk.IsPointInChunk(x, z, chunk.chunkCoordinates, chunk.chunkSize))
            {
                // Vector3 point = new Vector3(x, 0.0f, z);

                Vector3 point = Chunk.LocatePointInChunkAsPoint(x, z, chunk.chunkCoordinates, chunk.chunkSize);

                points.Add(point);
            }
        }
    }
}

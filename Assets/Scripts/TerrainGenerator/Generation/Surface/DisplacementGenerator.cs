using System;
using TerrainGenerator.Generation.Biome;
using TerrainGenerator.Generation.Structure;
using UnityEngine;

namespace TerrainGenerator.Generation.Surface
{
    public class DisplacementGenerator
    {
        public static DisplacementGenerator CreateDisplacementGenerator(Chunk chunk, int displacementInterblendLevel, BiomesDistribution biomesDistribution, BiomeGraphInterpreter biomeGraphInterpreter)
        {
            return new DisplacementGenerator(
                chunk,
                displacementInterblendLevel,
                biomesDistribution,
                biomeGraphInterpreter
            );
        }

        public readonly Chunk chunk;
        public readonly int displacementInterblendLevel;
        public readonly BiomesDistribution biomesDistribution;
        public readonly BiomeGraphInterpreter biomeGraphInterpreter;

        public DisplacementGenerator(Chunk chunk, int displacementInterblendLevel, BiomesDistribution biomesDistribution, BiomeGraphInterpreter biomeGraphInterpreter)
        {
            this.chunk = chunk;
            this.displacementInterblendLevel = displacementInterblendLevel;
            this.biomesDistribution = biomesDistribution;
            this.biomeGraphInterpreter = biomeGraphInterpreter;
        }

        public void ApplyDisplacementToMesh(Mesh mesh)
        {
            Vector3[] meshVertices = mesh.vertices;
            Vector3[] displacedVertices = new Vector3[meshVertices.Length];

            for (int vertexIndex = 0; vertexIndex < meshVertices.Length; vertexIndex++)
            {
                Vector3 vertexLocal = meshVertices[vertexIndex];
                Vector3 vertexGlobal = CalculateGlobalCoordinatesOfVertexLocal(vertexLocal);

                float weightedHeight = CalculateWeightedHeightInPoint(vertexGlobal);

                displacedVertices[vertexIndex] = new Vector3(
                    vertexLocal.x,
                    weightedHeight,
                    vertexLocal.z
                );
            }

            mesh.SetVertices(displacedVertices);
        }

        private Vector3 CalculateGlobalCoordinatesOfVertexLocal(Vector3 vertexLocal)
        {
            return Chunk.LocatePointInChunkAsPoint(
                vertexLocal.x,
                vertexLocal.z,
                chunk.chunkCoordinates,
                chunk.chunkSize);
        }

        private float CalculateWeightedHeightInPoint(Vector3 vertexGlobal)
        {
            BiomeSubcellCoordinate biomeSubcellCoordinate = biomesDistribution.LocatePointInBiomeSubgridAsBiomeSubcellCoordinates(vertexGlobal.x, vertexGlobal.z);
            BiomeSubsourcePoint biomeSubsourcePoint = biomesDistribution.LocateBiomeSubsourcePointByBiomeSubcellCoordinates(biomeSubcellCoordinate);
            BiomeSubsourcePoint[] biomeSubsourcePoints = biomesDistribution.CalculateSurroundingBiomeSubsourcePoints(biomeSubcellCoordinate);

            bool areAllSurroundingBiomesSame = AreAllSurroundingBiomesSame(biomeSubsourcePoint, biomeSubsourcePoints);

            if (areAllSurroundingBiomesSame)
            {
                float weightedHeight = GetHeightFromBiomeInVertexGlobal(vertexGlobal, biomeSubsourcePoint);

                return weightedHeight;
            }
            else
            {
                float[] distances = CalculateDistancesToBiomeSubsourcePointsFromVertexGlobal(vertexGlobal, biomeSubsourcePoint, biomeSubsourcePoints);
                float[] weights = CalculateWeightsOfDistances(distances);

                float[] heights = GetHeightsFromBiomesInVertexGlobal(vertexGlobal, biomeSubsourcePoint, biomeSubsourcePoints);

                float weightedHeight = CalculateWeightedHeight(weights, heights);

                return weightedHeight;
            }
        }

        private bool AreAllSurroundingBiomesSame(BiomeSubsourcePoint biomeSubsourcePoint, BiomeSubsourcePoint[] biomeSubsourcePoints)
        {
            foreach (var biomeSubsourcePointItem in biomeSubsourcePoints)
            {
                if (biomeSubsourcePointItem.biomeIndex != biomeSubsourcePoint.biomeIndex)
                {
                    return false;
                }
            }

            return true;
        }

        private float GetHeightFromBiomeInVertexGlobal(Vector3 vertexGlobal, BiomeSubsourcePoint biomeSubsourcePoint)
        {
            return biomeGraphInterpreter.GetBiomeHeight(
                biomeSubsourcePoint.biomeIndex,
                vertexGlobal.x,
                vertexGlobal.z);
        }

        private float[] CalculateDistancesToBiomeSubsourcePointsFromVertexGlobal(Vector3 vertexGlobal, BiomeSubsourcePoint biomeSubsourcePoint, BiomeSubsourcePoint[] biomeSubsourcePoints)
        {
            float[] distances = new float[9];

            distances[0] = CalculateDistanceBetweenPoints(
                vertexGlobal.x,
                vertexGlobal.z,
                biomeSubsourcePoint.x,
                biomeSubsourcePoint.z);

            for (int index = 0; index < biomeSubsourcePoints.Length; index++)
            {
                distances[index + 1] = CalculateDistanceBetweenPoints(
                    vertexGlobal.x,
                    vertexGlobal.z,
                    biomeSubsourcePoints[index].x,
                    biomeSubsourcePoints[index].z);
            }

            return distances;
        }

        private float CalculateDistanceBetweenPoints(float fromX, float fromZ, float toX, float toZ)
        {
            return Mathf.Sqrt(Mathf.Pow(fromX - toX, 2) - Mathf.Pow(fromZ - toZ, 2));
        }

        private float[] CalculateWeightsOfDistances(float[] distances)
        {
            float[] weights = new float[9];

            for (int index = 0; index < distances.Length; index++)
            {
                weights[index] = Weight(distances[index]);
            }

            return weights;
        }

        private float Weight(float distance)
        {
            return 1.0f / Mathf.Pow(distance, displacementInterblendLevel);
        }

        private float[] GetHeightsFromBiomesInVertexGlobal(Vector3 vertexGlobal, BiomeSubsourcePoint biomeSubsourcePoint, BiomeSubsourcePoint[] biomeSubsourcePoints)
        {
            float[] heights = new float[9];

            heights[0] = biomeGraphInterpreter.GetBiomeHeight(
                biomeSubsourcePoint.biomeIndex,
                vertexGlobal.x,
                vertexGlobal.z);

            for (int index = 0; index < biomeSubsourcePoints.Length; index++)
            {
                heights[index + 1] = biomeGraphInterpreter.GetBiomeHeight(
                biomeSubsourcePoints[index].biomeIndex,
                vertexGlobal.x,
                vertexGlobal.z);
            }

            return heights;
        }

        private float CalculateWeightedHeight(float[] weights, float[] heights)
        {
            float weightedHeight = 0.0f;

            float weightsSum = CalculateWeightsSum(weights);

            // E (weight / E weights_all * height_biome)

            for (int index = 0; index < weights.Length; index++)
            {
                weightedHeight += weights[index] / weightsSum * heights[index];
            }

            return weightedHeight;
        }

        private float CalculateWeightsSum(float[] weights)
        {
            float sum = 0.0f;

            foreach (var weight in weights)
            {
                sum += weight;
            }

            return sum;
        }
    }
}
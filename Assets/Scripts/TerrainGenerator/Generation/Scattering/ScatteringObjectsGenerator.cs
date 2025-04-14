using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Biomes.Scattering;
using TerrainGenerator.Generation.Biome;
using TerrainGenerator.Generation.Structure;
using UnityEngine;

namespace TerrainGenerator.Generation.Scattering
{
    public class ScatteringObjectsGenerator
    {
        public static ScatteringObjectsGenerator CreateScatteringObjectsGenerator(Chunk chunk, string seed, float scatteringInfluenceLevel, ScatteringPointsGenerator scatteringPointsGenerator, Dictionary<int, BiomeScatteringSettings[]> biomesScatteringSettings, BiomesDistribution biomesDistribution, BiomeGraphInterpreter biomeGraphInterpreter)
        {
            return new ScatteringObjectsGenerator(
                chunk,
                seed,
                scatteringInfluenceLevel,
                scatteringPointsGenerator,
                biomesScatteringSettings,
                biomesDistribution,
                biomeGraphInterpreter);
        }

        public readonly Chunk chunk;
        public readonly string seed;
        public readonly float scatteringInfluenceLevel;
        public readonly ScatteringPointsGenerator scatteringPointsGenerator;
        public readonly Dictionary<int, BiomeScatteringSettings[]> biomesScatteringSettings;
        public readonly BiomesDistribution biomesDistribution;
        public readonly BiomeGraphInterpreter biomeGraphInterpreter;
        
        private DeterministicRandom deterministicRandom;

        private float distanceLimit;

        private RaycastHit hitDown;
        private List<List<GameObject>> scatterings;
        private DetalizationLevel detalizationLevel;
        private List<GameObject> scattering;

        public ScatteringObjectsGenerator(Chunk chunk, string seed, float scatteringInfluenceLevel, ScatteringPointsGenerator scatteringPointsGenerator, Dictionary<int, BiomeScatteringSettings[]> biomesScatteringSettings, BiomesDistribution biomesDistribution, BiomeGraphInterpreter biomeGraphInterpreter)
        {
            this.chunk = chunk;
            this.seed = seed;
            this.scatteringInfluenceLevel = scatteringInfluenceLevel;
            this.scatteringPointsGenerator = scatteringPointsGenerator;
            this.biomesScatteringSettings = biomesScatteringSettings;
            this.biomesDistribution = biomesDistribution;
            this.biomeGraphInterpreter = biomeGraphInterpreter;
            deterministicRandom = new DeterministicRandom(seed);
            distanceLimit = CalculateDistanceLimit();
        }

        private float CalculateDistanceLimit()
        {
            return biomesDistribution.biomeSubcellSize * 2.83f * scatteringInfluenceLevel;
        }

        public List<List<GameObject>> CreateScatterings(DetalizationLevel detalizationLevel)
        {
            scatterings = new List<List<GameObject>>();
            this.detalizationLevel = detalizationLevel;

            LoopThroughEachBiomeSubsources();

            return scatterings;
        }

        private void LoopThroughEachBiomeSubsources()
        {
            foreach (int biomeIndex in biomesDistribution.biomeIndexToSubsources.Keys)
            {
                LoopThroughEachBiomeScattering(biomeIndex);
            }
        }

        private void LoopThroughEachBiomeScattering(int biomeIndex)
        {
            for (int scatteringIndex = 0; scatteringIndex < biomesScatteringSettings[biomeIndex].Length; scatteringIndex++)
            {
                if (IsNoScatteringsForBiome(biomeIndex, scatteringIndex))
                {
                    continue;
                }

                scattering = new List<GameObject>();

                List<Vector3> pointsRaw = CreateRawScatteringPoints(
                    detalizationLevel,
                    biomeIndex,
                    scatteringIndex);

                LoopThroughRawPointsOfBiomeScattering(biomeIndex, scatteringIndex, pointsRaw);

                scatterings.Add(scattering);
                scattering = null;
            }
        }

        private bool IsNoScatteringsForBiome(int biomeIndex, int scatteringIndex)
        {
            return biomesScatteringSettings[biomeIndex].Length - 1 < scatteringIndex;
        }

        private List<Vector3> CreateRawScatteringPoints(DetalizationLevel detalizationLevel, int biomeIndex, int scatteringIndex)
        {
            return scatteringPointsGenerator.CreatePoints(
                detalizationLevel.scattering,
                biomesScatteringSettings[biomeIndex][scatteringIndex]);
        }

        private void LoopThroughRawPointsOfBiomeScattering(int biomeIndex, int scatteringIndex, List<Vector3> pointsRaw)
        {
            for (int index = 0; index < pointsRaw.Count; index++)
            {
                Vector3 point = pointsRaw[index];

                // filter by distance to subsource points
                if(IsNotClosestSubsourcePointWithinDistanceLimit(biomeIndex, point))
                {
                    continue;
                }

                // filter by biome graph
                if (IsNotKeepPointByBiomeGraph(biomeIndex, scatteringIndex,  point))
                {
                    continue;
                }

                // filter by raycast "down"
                if(IsNotRaycastDownHitSurface(point, out Vector3 pointHit))
                {
                    continue;
                }

                // filter by height range
                if (IsNotPointHeightWithinScatteringHeightRange(biomeIndex, scatteringIndex, pointHit))
                {
                    continue;
                }

                // store scattering object
                GameObject scatteringObject = InstantiateScatteringObjectOnPointHit(
                    biomeIndex,
                    scatteringIndex,
                    pointHit
                );
                scattering.Add(scatteringObject);
            }
        }

        private bool IsNotClosestSubsourcePointWithinDistanceLimit(int biomeIndex, Vector3 point)
        {
            float[] distances = CalcuateDistancesToBiomeSubsourcePointsFromPoint(
                point,
                biomesDistribution.biomeIndexToSubsources[biomeIndex]);
            float distanceMin = CalculateDistanceMin(distances);

            return distanceMin > distanceLimit;
        }

        private float[] CalcuateDistancesToBiomeSubsourcePointsFromPoint(Vector3 point, List<BiomeSubsourcePoint> biomeSubsourcePoints)
        {
            float[] distances = new float[biomeSubsourcePoints.Count];

            for (int index = 0; index < distances.Length; index++)
            {
                distances[index] = CalculateDistanceBetweenPoints(
                    point.x,
                    point.z,
                    biomeSubsourcePoints[index].x,
                    biomeSubsourcePoints[index].z);
            }

            return distances;
        }

        private float CalculateDistanceBetweenPoints(float fromX, float fromZ, float toX, float toZ)
        {
            return Mathf.Sqrt(Mathf.Pow(fromX - toX, 2) + Mathf.Pow(fromZ - toZ, 2));
        }

        private float CalculateDistanceMin(float[] distances)
        {
            float distanceMin = float.MaxValue;

            foreach (var distance in distances)
            {
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                }
            }

            return distanceMin;    
        }

        private bool IsNotKeepPointByBiomeGraph(int biomeIndex, int scatteringIndex, Vector3 point)
        {
            bool isKeepPoint = biomeGraphInterpreter.GetBiomeScatteringIsKeepPoint(
                biomeIndex,
                scatteringIndex,
                point.x,
                point.z);

            return !isKeepPoint;
        }

        private bool IsNotRaycastDownHitSurface(Vector3 point, out Vector3 pointHit)
        {
            Vector3 rayOrigin = new Vector3(
                point.x,
                Mathf.Pow(10, 5),
                point.z
            );
            bool isHitDown = Physics.Raycast(rayOrigin, Vector3.down, out hitDown);
            pointHit = hitDown.point;

            return !isHitDown;
        }

        private bool IsNotPointHeightWithinScatteringHeightRange(int biomeIndex, int scatteringIndex, Vector3 pointHit)
        {
            bool result = 
                pointHit.y >= biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringMinHeight &&
                pointHit.y <= biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringMaxHeight;
            result =! result;

            return result;
        }

        private GameObject InstantiateScatteringObjectOnPointHit(int biomeIndex, int scatteringIndex, Vector3 pointHit)
        {
            GameObject scatteringObject = GameObject.Instantiate(
                biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringObject,
                pointHit,
                biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringObject.transform.rotation);

            // scale
            CalculateScatteringObjectScale(biomeIndex, scatteringIndex, pointHit, scatteringObject);
            // rotation
            CalculateScatteringObjectRotation(biomeIndex, scatteringIndex, pointHit, scatteringObject);

            return scatteringObject;
        }

        private void CalculateScatteringObjectScale(int biomeIndex, int scatteringIndex, Vector3 placePoint, GameObject scatteringObject)
        {
            float scale;
            if (biomesScatteringSettings[biomeIndex][scatteringIndex].isApplyScaleRange)
            {
                float rawOffsetScale = deterministicRandom.Value01(
                    biomeIndex * scatteringIndex,
                    placePoint.z * biomeIndex,
                    placePoint.x * scatteringIndex);
                scale =
                    biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringScaleMin +
                    rawOffsetScale *
                        (biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringScale -
                        biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringScaleMin);
            }
            else
            {
                scale = biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringScale;
            }
            scatteringObject.transform.localScale *= scale;
        }

        private void CalculateScatteringObjectRotation(int biomeIndex, int scatteringIndex, Vector3 placePoint, GameObject scatteringObject)
        {
            float rotation;
            if (biomesScatteringSettings[biomeIndex][scatteringIndex].isApplyRotationRange)
            {
                float rawOffsetRotation = deterministicRandom.Value01(
                    placePoint.x * biomeIndex,
                    biomeIndex * scatteringIndex,
                    placePoint.z * scatteringIndex);
                rotation =
                    biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringRotationMin +
                    rawOffsetRotation *
                        (biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringRotation -
                        biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringRotationMin);
            }
            else
            {
                rotation = biomesScatteringSettings[biomeIndex][scatteringIndex].scatteringRotation;
            }
            scatteringObject.transform.rotation = Quaternion.Euler(
                new Vector3(
                    0.0f,
                    rotation,
                    0.0f
                ));
        }
    }
}

using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Chunks;
using TerrainGenerator.Components.Settings.Biomes;
using TerrainGenerator.Generation.Structure;
using TerrainGenerator.Generation.Surface;
using TerrainGenerator.Generation.Water;
using TerrainGenerator.Generation.Scattering;
using UnityEngine;

namespace TerrainGenerator.Generation.Management
{
    public class ChunksGenerator
    {
        private static float localEpsilon = 1E-6f;

        public static ChunksGenerator CreateChunksGenerator(string seed, Transform generationCenter, ChunksSettings chunksSettings, BiomesSystemSettings biomesSystemSettings, GameObject generationParentGameObject)
        {
            return new ChunksGenerator(
                seed,
                generationCenter,
                chunksSettings,
                biomesSystemSettings,
                generationParentGameObject
            );
        }

        public readonly string seed;
        public readonly Transform generationCenter;
        public readonly ChunksSettings chunksSettings;
        public readonly BiomesSystemSettings biomesSystemSettings;
        public readonly GameObject generationParentGameObject;

        public readonly GeneratorSettingsInterpreter generatorSettingsInterpreter;

        // debug
        public Material materialSurface;
        public Material materialWater;

        private Dictionary<ChunkCoordinates, Chunk> chunks = new Dictionary<ChunkCoordinates, Chunk>();

        public ChunksGenerator(string seed, Transform generationCenter, ChunksSettings chunksSettings, BiomesSystemSettings biomesSystemSettings, GameObject generationParentGameObject)
        {
            this.seed = seed;
            this.generationCenter = generationCenter;
            this.chunksSettings = chunksSettings;
            this.biomesSystemSettings = biomesSystemSettings;
            this.generationParentGameObject = generationParentGameObject;
            generatorSettingsInterpreter = GeneratorSettingsInterpreter.CreateGeneratorSettingsInterpreter(
                seed,
                chunksSettings,
                biomesSystemSettings
            );
        }

        public void ScanForChunksUpdate()
        {
            for (float offsetX = 0.0f; offsetX <= chunksSettings.chunkPreloadMaxDistance; offsetX += chunksSettings.chunkSize - localEpsilon)
            {
                for (float offsetZ = 0.0f; offsetZ <= chunksSettings.chunkPreloadMaxDistance; offsetZ += chunksSettings.chunkSize - localEpsilon)
                {
                    CreateChunkDetalizationLevelBasedOnLODRange(offsetX, offsetZ);
                }
            }
        }

        private void CreateChunkDetalizationLevelBasedOnLODRange(float offsetX, float offsetZ)
        {
            for (int quadrantIndex = 1; quadrantIndex <= 4; quadrantIndex++)
            {
                ChunkCoordinates chunkCoordinates = CalculateChunkCoordinatesForQuadrant(
                    quadrantIndex,
                    offsetX,
                    offsetZ);

                bool isNoLODToShow = true;
                for (int indexLOD = 0; indexLOD < chunksSettings.chunkLODSettings.Length && isNoLODToShow; indexLOD++)
                {
                    Vector3 chunkCenter = Chunk.LocateCenterOfChunkAsPoint(
                        chunkCoordinates,
                        chunksSettings.chunkSize);

                    if (IsDistanceFromGenerationCenterToChunkCenterWithinLODRange(chunkCenter, indexLOD))
                    {
                        Chunk chunk = GetChunkInChunkCoordinates(chunkCoordinates);

                        GetAndShowChunkDetalizationLevel(chunk, indexLOD);

                        isNoLODToShow = false;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private ChunkCoordinates CalculateChunkCoordinatesForQuadrant(int quadrantIndex, float offsetX, float offsetZ)
        {
            int coeffX = 1;
            int coeffZ = 1;

            switch (quadrantIndex)
            {
                case 2:
                    coeffX *= -1;
                    break;
                case 3:
                    coeffX *= -1;
                    coeffZ *= -1;
                    break;
                case 4:
                    coeffZ *= -1;
                    break;
            }

            return Chunk.LocatePointAsChunkCoordinates(
                generationCenter.position.x + offsetX * coeffX,
                generationCenter.position.z + offsetZ * coeffZ,
                chunksSettings.chunkSize
            );
        }

        private Chunk GetChunkInChunkCoordinates(ChunkCoordinates chunkCoordinates)
        {
            if (chunks.ContainsKey(chunkCoordinates))
            {
                return chunks[chunkCoordinates];
            }
            else
            {
                return CreateChunkInChunkCoordinates(chunkCoordinates);
            }
        }

        private Chunk CreateChunkInChunkCoordinates(ChunkCoordinates chunkCoordinates)
        {
            // create
            Chunk chunk = Chunk.CreateChunk(
                chunksSettings.chunkSize,
                chunkCoordinates,
                chunksSettings.chunkLODSettings.Length,
                generationParentGameObject
            );

            // setup
            SurfaceMeshGenerator surfaceMeshGenerator = SurfaceMeshGenerator.CreateSurfaceMeshGenerator(
                chunk
            );
            chunk.AddSurfaceMeshGenerator(surfaceMeshGenerator);

            SurfaceDisplacementGenerator surfaceDisplacementGenerator = SurfaceDisplacementGenerator.CreateDisplacementGenerator(
                chunk,
                biomesSystemSettings.displacementInterblendLevel,
                biomesSystemSettings.displacementInfluenceLevel,
                generatorSettingsInterpreter.CreateBiomeDistribution(),
                generatorSettingsInterpreter.CreateBiomeGraphInterpreter()  // TODO: make an object pool for BiomeGraphInterpreter objects
            );
            chunk.AddSurfaceDisplacementGenerator(surfaceDisplacementGenerator);

            WaterMeshGenerator waterMeshGenerator = WaterMeshGenerator.CreateWaterMeshGenerator(
                chunk
            );
            chunk.AddWaterMeshGenerator(waterMeshGenerator);

            ScatteringObjectsGenerator scatteringObjectsGenerator = ScatteringObjectsGenerator.CreateScatteringObjectsGenerator(
                chunk,
                seed,
                biomesSystemSettings.scatteringInfluenceLevel,
                generatorSettingsInterpreter.GetBiomesScatteringSettings(),
                chunk.surfaceDisplacementGenerator.biomesDistribution,
                chunk.surfaceDisplacementGenerator.biomeGraphInterpreter
            );
            chunk.AddScatteringObjectsGenerator(scatteringObjectsGenerator);

            // store
            chunks.Add(chunkCoordinates, chunk);

            return chunk;
        }

        private bool IsDistanceFromGenerationCenterToChunkCenterWithinLODRange(Vector3 chunkCenter, int indexLOD)
        {
            float distanceSqared = (generationCenter.position - chunkCenter).sqrMagnitude;
            float rangeLODSqared = Mathf.Pow(chunksSettings.chunkLODSettings[indexLOD].maxDistance, 2.0f);

            return distanceSqared <= rangeLODSqared;
        }

        private void GetAndShowChunkDetalizationLevel(Chunk chunk, int indexLOD)
        {
            if (chunk.IsDetalizationLevelExists(indexLOD))
            {
                chunk.ShowDetalizationLevel(indexLOD);
                return;
            }

            // setup
            DetalizationLevel detalizationLevel = DetalizationLevel.CreateDetalizationLevel(
                indexLOD,
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).meshFillType,
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).meshResolution,
                chunk.chunkGameObject
            );
            chunk.AddDetalizationLevel(indexLOD, detalizationLevel);

            ObjectsScattering objectsScattering = ObjectsScattering.CreateScattering(
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).isApplyScatteringSparsing,
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).scatteringSparseLevel,
                detalizationLevel.detalizationLevelGameObject
            );
            detalizationLevel.AddScattering(objectsScattering);

            WaterCovering waterCovering = WaterCovering.CreateWaterCovering(
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).waterCoveringMeshFillType,
                generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).waterCoveringMeshResolution,
                detalizationLevel.detalizationLevelGameObject
            );
            detalizationLevel.AddWaterCovering(waterCovering);

            // generation
            Mesh surfaceMesh = chunk.surfaceMeshGenerator.CreateMesh(detalizationLevel);
            chunk.surfaceDisplacementGenerator.ApplyDisplacementToMesh(surfaceMesh);
            detalizationLevel.ApplySurfaceMesh(surfaceMesh, materialSurface);
            if (generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).isApplyCollision)
            {
                detalizationLevel.ApplySurfaceCollision(surfaceMesh);
            }
            MeshNormalsGenerator.RecalculateMeshNormals(surfaceMesh);


            if (generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).isApplyScattering)
            {
                List<List<GameObject>> scatterings = chunk.scatteringObjectsGenerator.CreateScatterings(detalizationLevel);
                for (int index = 0; index < scatterings.Count; index++)
                {
                    objectsScattering.ApplyScatteringGameObjects(scatterings[index]);
                }
            }

            if (generatorSettingsInterpreter.GetChunkLODSettings(indexLOD).isApplyWaterCovering)
            {
                Mesh waterMesh = chunk.waterMeshGenerator.CreateMesh(
                    detalizationLevel,
                    waterCovering
                );
                waterCovering.ApplyWaterCoveringMesh(waterMesh, materialWater);
            }

            RecalculateChunkDetalizationLevelNormals(chunk, indexLOD);

            // show up
            chunk.ShowDetalizationLevel(indexLOD);
        }

        private void RecalculateChunkDetalizationLevelNormals(Chunk chunk, int indexLOD)
        {
            for (int xOffset = -1; xOffset < 2; xOffset++)
            {
                for (int zOffset = -1; zOffset < 2; zOffset++)
                {
                    if (xOffset != 0 || zOffset != 0)
                    {
                        ChunkCoordinates chunkCoordinatesNext = new ChunkCoordinates(
                            chunk.chunkCoordinates.x + xOffset,
                            chunk.chunkCoordinates.z + zOffset);
                        if (chunks.ContainsKey(chunkCoordinatesNext))
                        {
                            if (chunks[chunkCoordinatesNext].IsDetalizationLevelExists(indexLOD))
                            {
                                MeshNormalsGenerator.RecalculateMeshEdgeNormals(
                                    indexLOD,
                                    chunk,
                                    chunks[chunkCoordinatesNext]
                                );
                            }
                        }
                    }
                }
            }
        }
    }
}
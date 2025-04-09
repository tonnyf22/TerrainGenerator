using TerrainGenerator.Components.Settings.Biomes;
using TerrainGenerator.Components.Settings.Chunks;
using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using TerrainGenerator.Generation.Structure;
using TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph;
using TerrainGenerator.Generation.Biome;
using TerrainGenerator.Generation.Surface;
using TerrainGenerator.Generation;
using TerrainGenerator.Generation.Water;

namespace TerrainGenerator.Components
{
    [DisallowMultipleComponent]
    public class TerrainGeneration : MonoBehaviour
    {
        public string seed;
        public Transform generationCenter;
        public ChunksSettings chunksSettings;
        public BiomesSystemSettings biomesSystemSettings;

        // debug
        public Material materialSurface;
        public Material materialWater;

        public Dictionary<ChunkCoordinates, Chunk> chunks = new Dictionary<ChunkCoordinates, Chunk>();

        void Start()
        {
            BiomeGraph[] biomeGraphs = CreateBiomeGraphs();
            BiomeGraphInterpreter biomeGraphInterpreter = BiomeGraphInterpreter.CreateBiomeGraphInterpreter(biomeGraphs);

            BiomesDistribution biomesDistribution = BiomesDistribution.CreateBiomesDistribution(
                biomesSystemSettings.biomesSettings.Length,
                seed,
                biomesSystemSettings.biomeGridCellSize,
                biomesSystemSettings.biomeSubgridCellSize);

            for (int coordinateX = 0; coordinateX < 16; coordinateX++)
            {
                for (int coordinateZ = 0; coordinateZ < 16; coordinateZ++)
                {
                    // chunk
                    Chunk chunk = Chunk.CreateChunk(
                        chunksSettings.chunkSize,
                        new ChunkCoordinates(coordinateX, coordinateZ),
                        gameObject);
                    chunks.Add(chunk.chunkCoordinates, chunk);
                    // surface mesh generator
                    SurfaceMeshGenerator surfaceMeshGenerator = new SurfaceMeshGenerator(chunk);
                    // surface displacement generator
                    SurfaceDisplacementGenerator surfaceDisplacementGenerator = SurfaceDisplacementGenerator.CreateDisplacementGenerator(
                        chunk,
                        biomesSystemSettings.displacementInterblendLevel,
                        biomesDistribution,
                        biomeGraphInterpreter);
                    chunk.AddSurfaceDisplacementGenerator(surfaceDisplacementGenerator);
                    // detalization level
                    DetalizationLevel detalizationLevel0 = DetalizationLevel.CreateDetalizationLevel(
                        0,
                        chunksSettings.chunkLODSettings[0].meshFillType,
                        chunksSettings.chunkLODSettings[0].meshResolution,
                        // true,
                        chunk.chunkGameObject);
                    chunk.AddDetalizationLevel(0, detalizationLevel0);
                    // water covering
                    WaterCovering waterCovering = WaterCovering.CreateWaterCovering(
                        chunksSettings.chunkLODSettings[0].waterCoveringMeshFillType,
                        chunksSettings.chunkLODSettings[0].waterCoveringMeshResolution,
                        detalizationLevel0.detalizationLevelGameObject
                    );
                    detalizationLevel0.AddWaterCovering(waterCovering);
                    // surface mesh
                    Mesh meshSurface = surfaceMeshGenerator.CreateMesh(detalizationLevel0);
                    surfaceDisplacementGenerator.ApplyDisplacementToMesh(meshSurface);
                    detalizationLevel0.ApplySurfaceCollision(meshSurface);
                    detalizationLevel0.ApplySurfaceMesh(meshSurface, materialSurface);
                    // surface normals
                    MeshNormalsGenerator.RecalculateMeshNormals(meshSurface);
                    for (int xIndex = -1; xIndex < 2; xIndex++)
                    {
                        for (int zIndex = -1; zIndex < 2; zIndex++)
                        {
                            if (xIndex != 0 || zIndex != 0)
                            {
                                ChunkCoordinates chunkCoordinatesNext = new ChunkCoordinates(
                                    chunk.chunkCoordinates.x + xIndex,
                                    chunk.chunkCoordinates.z + zIndex);
                                if (chunks.ContainsKey(chunkCoordinatesNext))
                                {
                                    MeshNormalsGenerator.RecalculateMeshEdgeNormals(
                                        0,
                                        chunk,
                                        chunks[chunkCoordinatesNext]
                                    );
                                }
                            }
                        }
                    }
                    // water mesh
                    WaterMeshGenerator waterMeshGenerator = WaterMeshGenerator.CreateWaterMeshGenerator(chunk);
                    chunk.AddWaterMeshGenerator(waterMeshGenerator);
                    Mesh meshWaterCovering = waterMeshGenerator.CreateMesh(
                        detalizationLevel0,
                        waterCovering);
                    // waterCovering.ApplyWaterCoveringCollision(meshWaterCovering);
                    waterCovering.ApplyWaterCoveringMesh(meshWaterCovering, materialWater);
                }
            }
        }

        private BiomeGraph[] CreateBiomeGraphs()
        {
            BiomeGraph[] biomeGraphs = new BiomeGraph[biomesSystemSettings.biomesSettings.Length];
            int index = 0;
            foreach (BiomeSettings biomeSettings in biomesSystemSettings.biomesSettings)
            {
                biomeGraphs[index] = biomeSettings.biomeNodeGraph;
                index++;
            }
            return biomeGraphs;
        }

        void Update()
        {
            
        }
    }
}

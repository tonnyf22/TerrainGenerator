using System;
using System.Collections.Generic;
using TerrainGenerator.Components.Settings.Chunks;
using TerrainGenerator.Generation.Structure;
using UnityEngine;

namespace TerrainGenerator.Generation.Surface
{
    public static class MeshGenerator
    {
        public static Mesh CreateMesh(Chunk chunk, DetalizationLevel detalizationLevel)
        {
            Mesh mesh = new Mesh();

            GenerateVertices(mesh, chunk.chunkSize, detalizationLevel.meshResolution);
            GenerateTriangles(mesh, detalizationLevel.meshFillType, detalizationLevel.meshResolution);

            RecalculateMeshAttributes(mesh);

            return mesh;
        }

        private static void GenerateVertices(Mesh mesh, float chunkSize, int meshResolution)
        {
            List<Vector3> vertices = new List<Vector3>();
            float verticesGapSize = chunkSize / (meshResolution - 1);

            for (int xIndex = 0; xIndex < meshResolution; xIndex++)
            {
                for (int zIndex = 0; zIndex < meshResolution; zIndex++)
                {
                    float xCoordinate = xIndex * verticesGapSize;
                    float zCoordinate = zIndex * verticesGapSize;

                    Vector3 vertex = new Vector3(xCoordinate, 0.0f, zCoordinate);

                    vertices.Add(vertex);
                }
            }

            mesh.SetVertices(vertices);
        }

        private static void GenerateTriangles(Mesh mesh, MeshFillType meshFillType, int meshResolution)
        {
            switch (meshFillType)
            {
                case MeshFillType.QuadGrid:
                    GenerateTrianglesQuadGridMesh(mesh, meshResolution);
                    break;
                case MeshFillType.QuadGridStars:
                    GenerateTrianglesQuadGridStarsMesh(mesh, meshResolution);
                    break;
                case MeshFillType.QuadGridZigzag:
                    GenerateTrianglesQuadGridZigzagMesh(mesh, meshResolution);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(meshFillType),
                        meshFillType,
                        "Unsupported FillType.");
            }
        }

        private static void GenerateTrianglesQuadGridMesh(Mesh mesh, int meshResolution)
        {
            List<int> triangles = new List<int>();

            for (int xIndex = 0; xIndex < meshResolution - 1; xIndex++)
            {
                for (int zIndex = 0; zIndex < meshResolution - 1; zIndex++)
                {
                    int v1 = xIndex * meshResolution + zIndex;
                    int v2 = xIndex * meshResolution + zIndex + 1;
                    int v3 = (xIndex + 1) * meshResolution + zIndex;
                    int v4 = (xIndex + 1) * meshResolution + zIndex + 1;

                    triangles.AddRange(
                        new int[]{
                            v1, v2, v4,
                            v4, v3, v1
                        });
                }
            }

            mesh.SetTriangles(triangles, 0, false);
        }

        private static void GenerateTrianglesQuadGridStarsMesh(Mesh mesh, int meshResolution)
        {
            List<int> triangles = new List<int>();

            for (int xIndex = 0; xIndex < meshResolution - 1; xIndex++)
            {
                for (int zIndex = 0; zIndex < meshResolution - 1; zIndex++)
                {
                    int v1 = xIndex * meshResolution + zIndex;
                    int v2 = xIndex * meshResolution + zIndex + 1;
                    int v3 = (xIndex + 1) * meshResolution + zIndex;
                    int v4 = (xIndex + 1) * meshResolution + zIndex + 1;

                    if (xIndex % 2 == zIndex % 2)
                    {
                        triangles.AddRange(
                            new int[]{
                                v1, v2, v4,
                                v4, v3, v1
                            });
                    }
                    else // if (xIndex % 2 != zIndex % 2)
                    {
                        triangles.AddRange(
                            new int[]{
                                v3, v1, v2,
                                v2, v4, v3
                            });
                    }
                }
            }

            mesh.SetTriangles(triangles, 0, false);
        }

        private static void GenerateTrianglesQuadGridZigzagMesh(Mesh mesh, int meshResolution)
        {
            List<int> triangles = new List<int>();

            for (int xIndex = 0; xIndex < meshResolution - 1; xIndex++)
            {
                for (int zIndex = 0; zIndex < meshResolution - 1; zIndex++)
                {
                    int v1 = xIndex * meshResolution + zIndex;
                    int v2 = xIndex * meshResolution + zIndex + 1;
                    int v3 = (xIndex + 1) * meshResolution + zIndex;
                    int v4 = (xIndex + 1) * meshResolution + zIndex + 1;

                    if (zIndex % 2 == 0)
                    {
                        triangles.AddRange(
                            new int[]{
                                v1, v2, v4,
                                v4, v3, v1
                            });
                    }
                    else // if (zIndex % 2 == 1)
                    {
                        triangles.AddRange(
                            new int[]{
                                v3, v1, v2,
                                v2, v4, v3
                            });
                    }
                }
            }

            mesh.SetTriangles(triangles, 0, false);
        }

        // maybe move to outer class like "NormalsRecalculationManager"
        public static void RecalculateMeshAttributes(Mesh mesh)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }
    }
}
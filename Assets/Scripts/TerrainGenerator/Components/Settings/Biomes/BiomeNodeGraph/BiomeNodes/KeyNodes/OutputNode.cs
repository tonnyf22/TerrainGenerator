using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.KeyNodes
{
	[CreateNodeMenu("Key Nodes/Output"), NodeWidth(250)]
	public class OutputNode : Node
	{
		[Input] public float Height;
		[Input] public Vector3[] ScatteringPoints1;
		[Input] public Vector3[] ScatteringPoints2;
		[Input] public Vector3[] ScatteringPoints3;

		[HideInInspector] public float height;
		[HideInInspector] public Vector3[] scatteringPoints1;
		[HideInInspector] public Vector3[] scatteringPoints2;
		[HideInInspector] public Vector3[] scatteringPoints3;
	}
}
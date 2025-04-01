using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.KeyNodes
{
	[CreateNodeMenu("Key Nodes/Output"), NodeWidth(250)]
	public class OutputNode : Node
	{
		[Input] public float height;

		public float GetHeightOutput()
		{
			GetInputValue("height", height);
			return height;
		}
	}
}
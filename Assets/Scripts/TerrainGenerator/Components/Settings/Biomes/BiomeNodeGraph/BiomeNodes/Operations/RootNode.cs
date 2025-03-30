using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Root")]
	public class RootNode : Node
	{
		[Input] public float input;
		[Input] public float index;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Root();
			}
			else
			{
				return null;
			}
		}

		private float Root()
		{
			GetInputValue("input", input);
			GetInputValue("index", index);

			return math.pow(input, 1.0f / index);
		}
	}
}
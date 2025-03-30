using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Sine")]
	public class SineNode : Node
	{
		[Input] public float input;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Sine();
			}
			else
			{
				return null;
			}
		}

		private float Sine()
		{
			GetInputValue("input", input);

			return math.sin(input);
		}
	}
}
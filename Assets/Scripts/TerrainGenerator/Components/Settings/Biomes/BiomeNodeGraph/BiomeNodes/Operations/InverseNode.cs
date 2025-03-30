using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Inverse")]
	public class InverseNode : Node
	{
		[Input] public float input;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Inverse();
			}
			else
			{
				return null;
			}
		}

		private float Inverse()
		{
			GetInputValue("input", input);

			return input * (-1);
		}
	}
}
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Multiply")]
	public class MultiplyNode : Node
	{
		[Input] public float inputA;
		[Input] public float inputB;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Multiply();
			}
			else
			{
				return null;
			}
		}

		private float Multiply()
		{
			GetInputValue("inputA", inputB);
			GetInputValue("inputB", inputB);

			return inputA * inputB;
		}
	}
}
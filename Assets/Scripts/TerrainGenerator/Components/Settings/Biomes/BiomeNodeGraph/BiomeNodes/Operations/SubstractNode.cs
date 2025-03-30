using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Substract")]
	public class SubstractNode : Node
	{
		[Input] public float inputA;
		[Input] public float inputB;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Substract();
			}
			else
			{
				return null;
			}
		}

		private float Substract()
		{
			GetInputValue("inputA", inputA);
			GetInputValue("inputB", inputB);

			return inputA - inputB;
		}
	}
}
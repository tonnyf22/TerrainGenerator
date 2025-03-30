using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Divide")]
	public class DivideNode : Node
	{
		[Input] public float inputA;
		[Input] public float inputB;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Divide();
			}
			else
			{
				return null;
			}
		}

		private float Divide()
		{
			GetInputValue("inputA", inputA);
			GetInputValue("inputB", inputB);

			return inputA / inputB;
		}
	}
}
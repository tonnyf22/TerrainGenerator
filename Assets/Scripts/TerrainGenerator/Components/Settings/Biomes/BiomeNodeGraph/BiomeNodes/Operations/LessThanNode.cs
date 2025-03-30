using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Less Than")]
	public class LessThanNode : Node
	{
		[Input] public float inputA;
		[Input] public float inputB;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return LessThan();
			}
			else
			{
				return null;
			}
		}

		private float LessThan()
		{
			GetInputValue("inputA", inputA);
			GetInputValue("inputB", inputB);

			switch (inputA < inputB)
			{
				case true:
					return 1.0f;
				case false:
					return 0.0f;
			}
		}
	}
}
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Greater Than")]
	public class GreaterThanNode : Node
	{
		[Input] public float inputA;
		[Input] public float inputB;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return GreaterThan();
			}
			else
			{
				return null;
			}
		}

		private float GreaterThan()
		{
			GetInputValue("inputA", inputB);
			GetInputValue("inputB", inputB);

			switch (inputA > inputB)
			{
				case true:
					return 1.0f;
				case false:
					return 0.0f;
			}
		}
	}
}
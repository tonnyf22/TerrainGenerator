using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Logarithm")]
	public class LogarithmNode : Node
	{
		[Input] public float input;
		[Input] public float _base;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Logarithm();
			}
			else
			{
				return null;
			}
		}

		private float Logarithm()
		{
			GetInputValue("input", input);
			GetInputValue("base", _base);

			return math.log(input) / math.log(_base);
		}
	}
}
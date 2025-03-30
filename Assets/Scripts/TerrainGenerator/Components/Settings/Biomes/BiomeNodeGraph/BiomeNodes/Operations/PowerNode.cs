using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Power")]
	public class PowerNode : Node
	{
		[Input] public float input;
		[Input] public float power;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Power();
			}
			else
			{
				return null;
			}
		}

		private float Power()
		{
			GetInputValue("input", input);
			GetInputValue("power", power);

			return math.pow(input, power);
		}
	}
}
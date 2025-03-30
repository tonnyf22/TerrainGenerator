using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Clamp")]
	public class ClampNode : Node
	{
		[Input] public float input;

		[Input] public float min;
		[Input] public float max;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Clamp();
			}
			else
			{
				return null;
			}
		}

		private float Clamp()
		{
			GetInputValue("input", input);
			GetInputValue("min", min);
			GetInputValue("max", max);

			if (input < min)
			{
				return min;
			}
			else if (input > max)
			{
				return max;
			}
			else
			{
				return input;
			}
		}
	}
}
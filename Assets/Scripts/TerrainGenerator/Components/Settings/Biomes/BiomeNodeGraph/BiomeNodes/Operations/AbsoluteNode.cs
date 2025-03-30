﻿using Unity.Mathematics;
using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.Operations
{
	[CreateNodeMenu("Operations/Absolute")]
	public class AbsoluteNode : Node
	{
		[Input] public float input;

		[Output] public float value;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "value")
			{
				return Absolute();
			}
			else
			{
				return null;
			}
		}

		private float Absolute()
		{
			GetInputValue("input", input);

			return math.abs(input);
		}
	}
}
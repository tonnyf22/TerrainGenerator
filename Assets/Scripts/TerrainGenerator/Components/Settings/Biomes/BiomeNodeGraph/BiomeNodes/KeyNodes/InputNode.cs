using UnityEngine;
using XNode;

namespace TerrainGenerator.Components.Settings.Biomes.BiomeNodeGraph.BiomeNodes.KeyNodes
{
	[CreateNodeMenu("Key Nodes/Input")]
	public class InputNode : Node
	{
		[HideInInspector] public float inputPointX;
		[HideInInspector] public float inputPointZ;

		[HideInInspector] public float chunkSize;
		[HideInInspector] public float chunkCoordinatesX;
		[HideInInspector] public float chunkCoordinatesZ;

		[Output] public float InputPointX;
		[Output] public float InputPointZ;

		[Output] public float ChunkSize;
		[Output] public float ChunkCoordinatesX;
		[Output] public float ChunkCoordinatesZ;

		public override object GetValue(NodePort port)
		{
			if (port.fieldName == "inputPointX")
			{
				return InputPointX;
			}
			else if (port.fieldName == "inputPointZ")
			{
				return InputPointZ;
			}
			else if (port.fieldName == "chunkSize")
			{
				return ChunkSize;
			}
			else if (port.fieldName == "chunkCoordinatesX")
			{
				return ChunkCoordinatesX;
			}
			else if (port.fieldName == "chunkCoordinatesZ")
			{
				return ChunkCoordinatesX;
			}
			else
			{
				return null;
			}
		}
	}
}
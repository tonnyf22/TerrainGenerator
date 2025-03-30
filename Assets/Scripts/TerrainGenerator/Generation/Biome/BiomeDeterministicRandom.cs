using System;

namespace TerrainGenerator.Generation.Biome
{
    public class BiomeDeterministicRandom
    {
        public readonly string seed;
        public readonly int seedInt;

        public BiomeDeterministicRandom(string seed)
        {
            this.seed = seed;
            seedInt = TransformSeedStringToInt(seed);
        }

        private int TransformSeedStringToInt(string seed)
        {
            return seed.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        // 0-1
        public float Value01(float x1)
        {
            float[] data = new float[] { x1 };
            float value = Value01(data);

            return value;
        }

        // 0-1
        public float Value01(float x1, float x2)
        {
            float[] data = new float[] { x1, x2 };
            float value = Value01(data);

            return value;
        }

        // 0-1
        public float Value01(float x1, float x2, float x3)
        {
            float[] data = new float[] { x1, x2, x3 };
            float value = Value01(data);

            return value;
        }

        private float Value01(float[] inputData)
        {
            return NormalizedHashFromData(inputData);
        }

        private float NormalizedHashFromData(float[] inputData)
        {
            int[] xRawInts = FloatsToRawInts(inputData);

            int hash;
            hash = MakeInitialCombinedHash(xRawInts);
            hash = XORShift(hash);
            hash = MakePositiveOfHash(hash);

            float normalizedHash = NormalizeHash(hash);
            return normalizedHash;
        }

        private int[] FloatsToRawInts(float[] inputData)
        {
            int[] xRawInts = new int[inputData.Length];

            for (int index = 0; index < xRawInts.Length; index++)
            {
                xRawInts[index] = BitConverter.SingleToInt32Bits(inputData[index]);
            }

            return xRawInts;
        }

        private int MakeInitialCombinedHash(int[] xRawInts)
        {
            int hash = 0;
            for (int index = 0; index < xRawInts.Length; index++)
            {
                hash ^= 
                    xRawInts[index] * 
                    seedInt;
            }
            return hash;
        }

        private int XORShift(int hash)
        {
            hash ^= hash >> 13;
            hash ^= hash << 13;
            hash ^= hash >> 17;
            hash ^= hash << 5;
            return hash;
        }

        private int MakePositiveOfHash(int hash)
        {
            return hash & 0x7FFFFFFF;
        }

        private float NormalizeHash(int hash)
        {
            return hash / (float)int.MaxValue;
        }
    }
}

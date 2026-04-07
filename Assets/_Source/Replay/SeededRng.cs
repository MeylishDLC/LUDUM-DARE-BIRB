using System;

namespace Replay
{
    public sealed class SeededRng : IRng
    {
        private Random _random;

        public int InitialSeed { get; private set; }

        public SeededRng(int seed)
        {
            Reset(seed);
        }

        public void Reset(int seed)
        {
            InitialSeed = seed;
            _random = new Random(seed);
        }

        public int RangeInt(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
            {
                return minInclusive;
            }

            return _random.Next(minInclusive, maxExclusive);
        }

        public float RangeFloat(float minInclusive, float maxExclusive)
        {
            var t = (float)_random.NextDouble();
            return minInclusive + (maxExclusive - minInclusive) * t;
        }
    }
}

namespace Replay
{
    public interface IRng
    {
        int InitialSeed { get; }

        void Reset(int seed);

        int RangeInt(int minInclusive, int maxExclusive);

        float RangeFloat(float minInclusive, float maxExclusive);
    }
}

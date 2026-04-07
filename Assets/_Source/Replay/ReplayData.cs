using System.Collections.Generic;

namespace Replay
{
    public sealed class ReplayData
    {
        public int Seed;
        public int TickRateHz;
        public int MaxRecordedTicks;
        public readonly List<ReplayCommand> Commands = new();
    }
}

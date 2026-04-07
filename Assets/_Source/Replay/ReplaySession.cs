namespace Replay
{
    public static class ReplaySession
    {
        public static bool HasPendingReplay { get; private set; }

        private static ReplayData pendingReplay;
        private static int? pendingSeed;

        public static void SetPendingReplayPlayback(ReplayData data, int seed)
        {
            pendingReplay = data;
            pendingSeed = seed;
            HasPendingReplay = true;
        }

        public static bool TryConsumePendingReplay(out ReplayData data)
        {
            if (pendingReplay == null)
            {
                data = null;
                return false;
            }

            data = pendingReplay;
            pendingReplay = null;
            HasPendingReplay = false;
            return true;
        }

        public static bool TryConsumePendingSeed(out int seed)
        {
            if (pendingSeed == null)
            {
                seed = 0;
                return false;
            }

            seed = pendingSeed.Value;
            pendingSeed = null;
            return true;
        }
    }
}

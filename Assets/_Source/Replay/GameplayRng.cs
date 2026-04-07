namespace Replay
{ 
    public static class GameplayRng
    {
        public static IRng Instance { get; internal set; }

        public static IRng Obstacles { get; internal set; }
        public static IRng Collectables { get; internal set; }
    }
}

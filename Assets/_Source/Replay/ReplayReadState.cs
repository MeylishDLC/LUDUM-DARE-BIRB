namespace Replay
{
    public sealed class ReplayReadState : IReplayReadState
    {
        public bool IsReplaying { get; internal set; }

        public float ReplayMoveX { get; internal set; }
    }
}
